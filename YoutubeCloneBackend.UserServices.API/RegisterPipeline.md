# 📘 Registration Pipeline – Documentation

This document provides the complete end-to-end flow of the User Registration Process including sending OTP, verifying OTP, creating the user, publishing events, and sending welcome emails.

# 🟦 1. Request Registration OTP – /api/User/Register
### **Flow**
```
User → API Gateway → User Service → Register Controller → Insert/Update Temp User →
Generate OTP → Store OTP → Publish Event → RabbitMQ → Send OTP Email
```

Documentation will be updated soon.
