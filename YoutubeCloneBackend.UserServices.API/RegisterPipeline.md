# 📘 Registration Pipeline – Documentation

This document provides the complete end-to-end flow of the User Registration Process including sending OTP, verifying OTP, creating the user, publishing events, and sending welcome emails.

# 🟦 1. Request Registration OTP – /api/User/Register
### **Flow**
```
User → API Gateway → User Service → Register Controller → Insert/Update Temp User →
Generate OTP → Store OTP → Publish Event → RabbitMQ → Send OTP Email
```

Documentation will be updated soon.

<img width="5167" height="1565" alt="RegisterController" src="https://github.com/user-attachments/assets/29115a90-2305-4bb6-bce0-15d70d878253" />

<img width="4676" height="1427" alt="VerifyOTP" src="https://github.com/user-attachments/assets/229daf69-25f5-4b1c-8102-8a07975a4495" />

<img width="5273" height="698" alt="CreatePassword" src="https://github.com/user-attachments/assets/a8dad7c1-4529-443d-992b-0015b9ee1cbb" />
