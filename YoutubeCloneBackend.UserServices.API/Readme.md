# 1. RegisterController (Implemented)

### When a user hits `/api/Register`:

1️⃣ **User Service validates the email and checks if the user already exists.**  
2️⃣ If it’s a new user, the service inserts them into a Temp Table (not the actual User table).  
3️⃣ Then it securely calls the SmsEmail Service using an internal API key.  
4️⃣ SmsEmail Service generates an OTP, applies rate-limiting, cooldown windows, and expiration logic all inside PostgreSQL functions.  
5️⃣ The OTP metadata (expiry time, remaining attempts, retry-after) is returned synchronously to the User Service.  
6️⃣ In parallel, the same service publishes an async event to RabbitMQ so that email delivery happens in the background.  
7️⃣ A background worker inside SmsEmailService consumes the message and sends the email using Mailjet.

### 🔐 Key things I implemented

✔ Microservice separation (User Service + SmsEmail Service)  
✔ Internal API authentication using custom headers  
✔ Temp Table user onboarding  
✔ PostgreSQL function to handle:  
&nbsp;&nbsp;&nbsp;&nbsp;• Storing OTP  
&nbsp;&nbsp;&nbsp;&nbsp;• attempt counting  
&nbsp;&nbsp;&nbsp;&nbsp;• cooldowns  
&nbsp;&nbsp;&nbsp;&nbsp;• OTP expiry  
✔ RabbitMQ event publishing + background consumer  
✔ Mailjet integration for actual OTP email delivery  
✔ Proper synchronous + asynchronous flow separation  
✔ Error handling  
✔ Clean architecture and API boundaries  

### RegisterController Work Flow

<img width="1943" height="1067" alt="RegisterController"
src="https://github.com/user-attachments/assets/b6610adc-19c2-453d-b65f-8d8026929a9d" />

---

# 2. VerifyOTPController (Partially Implemented)

### When a user hits `/api/VerifyOtp`:

1️⃣ User Service validates the email, OTP, and the verification purpose.  
2️⃣ It checks the OTP against the Temp Users Table using PostgreSQL logic (expiry, attempts, cooldown).  
3️⃣ If the OTP is valid, the user is promoted from **Temp Users → Users** table.  
4️⃣ The temp entry is removed so half-registered accounts don’t stay around.  
5️⃣ An event is published to RabbitMQ after successful verification.  
6️⃣ SmsEmailService picks up the event and sends a welcome email asynchronously.  
7️⃣ If the OTP is invalid, expired, or attempts are exhausted, the service returns the correct retry/cooldown info instantly.

Right now, this flow is implemented **for Register purpose only** —  
Login and Reset Password verification will be added next.

### 🔐 Key Things I Implemented

✔ OTP verification logic inside User Service  
✔ Purpose-based handling (Register implemented; Login & Reset coming soon)  
✔ PostgreSQL function for matching OTP, expiry, attempts, cooldowns  
✔ Promotion of **TempUser → User** on success  
✔ Cleanup of temp users table entries  
✔ RabbitMQ event publishing after successful verification  
✔ Asynchronous welcome email via SmsEmailService background worker  
✔ Clean synchronous + asynchronous separation  
✔ Proper error and cooldown responses

### VerifyOTPController Work Flow

<img width="2749" height="1213" alt="VerifyOTPController" 
src="https://github.com/user-attachments/assets/d2c1e2d9-b502-4102-9400-27aae80b810d" />

---

# Proposed User Service Design

<img width="4787" height="5405" alt="User Service"
src="https://github.com/user-attachments/assets/fe3ccea3-a5f1-45ef-87fb-1203dba7cd66" />
