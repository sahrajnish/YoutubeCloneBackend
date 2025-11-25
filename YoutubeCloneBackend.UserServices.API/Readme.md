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

# Proposed User Service Design

<img width="4787" height="5405" alt="User Service"
src="https://github.com/user-attachments/assets/fe3ccea3-a5f1-45ef-87fb-1203dba7cd66" />
