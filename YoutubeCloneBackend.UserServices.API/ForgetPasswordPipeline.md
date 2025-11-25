# 📘 Forgot Password Pipeline – Documentation

The **Forgot Password Pipeline** handles the complete password recovery flow in a secure, event-driven, rate-limited, and OTP-based manner.

**Contents**
1. [Request Reset OTP – `/api/User/ForgetPassword`](#-1-apiuserforgetpassword--request-reset-otp)
2. [Verify Reset OTP – `/api/User/ForgetPassword/VerifyOtp`](#-2-apiuserforgetpasswordverifyotp--verify-reset-otp)
3. [Create New Password – `/api/User/ForgetPassword/CreatePassword`](#-3-apiuserforgetpasswordcreatepassword--create-new-password)
4. [Database Structure](#️-database-structure)
5. [Event-Driven Notifications](#-event-driven-notifications)
6. [Security Features](#-security-features)


This document explains the flow in **3 clear stages**:

1. **Request Password Reset (Send OTP)**
2. **Verify Reset OTP**
3. **Create New Password**

Each stage uses PostgreSQL functions, RabbitMQ events, throttling logic, and notification emails.

# 🟦 1. /api/User/ForgetPassword — Request Reset OTP

### **Flow**
```
User → API Gateway → User Service → Validate Email → fn_send_reset_otp →
Store OTP → Publish Event → RabbitMQ → Send Email
```

### **Steps**
1. User requests "Forgot Password".
2. Backend calls PostgreSQL function:
```
auth.fn_send_reset_otp('resetpassword', email, otp)
```
3. DB checks:
```
- User exists
- Throttle resend attempts
- Handle cooldowns
- Insert/Update OTP row
- Set otp_expires_at = NOW() + 10 min
```
4. OTP stored in **reset_otps**.
5. Event published to RabbitMQ.
6. Email consumer sends OTP mail.

<img width="3777" height="1172" alt="ForgetPassword" src="https://github.com/user-attachments/assets/72119f64-b2c3-4afc-9bbf-70e2c4e1f826" />

### **Possible Responses**
- User does not exists.
- You have not created a password yet.
- OTP created successfully.
- Cooldown active. Try again later.
- Maximum attempts reached. Try after cooldown.
- New OTP generated successfully.

# 🟦 2. /api/User/ForgetPassword/VerifyOtp — Verify OTP

### **Flow**
```
User → API Gateway → User Service → VerifyOtpService → fn_reset_otp_validation →
Mark is_used = TRUE → Set password_reset_allowed_till
```


### **Steps**
1. User submits OTP.
2. Backend calls:
```
auth.fn_reset_otp_validation(purpose, email, otp)
```
3. DB validates:
```
- Row exists in reset_otps
- Purpose is `resetpassword`
- OTP matches
- OTP not used before
- OTP not expired
- Cooldown rules
- Max 3 attempts limit
```

4. On success:
```
is_used = TRUE
password_reset_allowed_till = NOW() + 15 minutes
```
5. Publish success notification event.

<img width="5114" height="1676" alt="VerifyOTP" src="https://github.com/user-attachments/assets/8a5210fa-e3d4-4331-9f52-4ad62f140d4b" />

### **Possible Responses**
- User does not exist.
- Generate OTP before verification.
- OTP does not match purpose.
- OTP already used.
- Cooldown expired. Regenerate OTP.
- Cooldown active. Try again after cooldown.
- OTP expired.
- Max attempt reached. Try again after cooldown.
- Incorrect OTP
- OTP validation successful.

# 🟦 3. /api/User/ForgetPassword/CreatePassword — Create New Password

### **Flow**
```
User → API Gateway → User Service → CreatePasswordService → fn_reset_password →
Update password_hash → Remove OTP row → Publish notification → Email consumer
```

### **Steps**
1. User enters new password.
2. Service validates:
```
- Email format
- Strong password rules
- Confirm password match
```
3. Calls PostgreSQL:
```
user_srvc.fn_reset_password(purpose, email, new_password_hash)
```

4. DB checks:
- User exists
- Reset OTP row exists
- Purpose matches
- OTP was used (`is_used = TRUE`)
- OTP not expired
- password_reset_allowed_till > NOW()

5. On success:
- password_hash updated
- password_changed_at set
- OTP row deleted
- Notification event sent

<img width="5489" height="1495" alt="CreatePassword" src="https://github.com/user-attachments/assets/4b683533-c511-4778-a1be-87ae63ce5682" />

### **Possible Responses**
- Password updated successfully
- OTP not verified
- Verification window expired
- Invalid purpose
- User not found

# 🗄️ Database Structure

### **Table: auth.reset_otps**
| Column | Description |
|--------|------------|
| id | Primary Key |
| user_id | FK to users table |
| otp_code | OTP value |
| otp_attempts | Attempts for OTP verification |
| otp_created_at | Timestamp |
| otp_expires_at | OTP expiration timestamp |
| otp_reattempt_at | OTP verify cooldown |
| purpose | `resetpassword` or `unlock` |
| is_used | True when OTP is consumed |
| resend_attempts | Attempts to resend OTP |
| resend_reattempt_at | Resend cooldown |
| password_reset_allowed_till | Validity window for creating new password |

# 📬 Event-Driven Notifications

### **1️. OTP Event** 
Published during “Send OTP” step:
```json
{
  "Purpose": "resetpassword",
  "Email": "user@mail.com",
  "Otp": "123456"
}
```

### **2. Notification Event** 
Published during “Create New Password” step:
```json
{
  "Purpose": "resetpassword",
  "Email": "user@mail.com"
}
```

# 🔐 Security Features
1. Password hashing
2. OTP expiry (10 minutes)
3. Purpose-bound OTP validation
4. Rate limiting for resend & verify attempts
5. Cooldown timers
6. OTP cannot be reused
7. OTP deleted after successful password reset
8. `password_reset_allowed_till` adds extra protection
