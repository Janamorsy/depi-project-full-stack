# Nile Care

A full-stack medical tourism platform that connects international patients with healthcare providers in Egypt. The platform allows users to book doctor appointments, find accessible hotels, arrange transportation, and manage their medical journey all in one place.

## What This Project Does

Nile Care helps medical tourists plan their entire trip to Egypt for healthcare services. Patients can browse verified doctors by specialty, book appointments, chat with their healthcare providers, upload medical records, and arrange accommodation and transport. The platform also includes portals for doctors to manage their appointments, hotel owners to list their properties, and administrators to oversee the entire system.

## Tech Stack

**Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core, SQL Server, ASP.NET Identity

**Frontend:** React 18, Vite, Bootstrap 5, React Router

**Payments:** Stripe integration for appointment and booking payments

**Authentication:** JWT tokens with role-based access for Users, Doctors, Hotel Owners, and Admins

---

## Getting Started

### Prerequisites

Make sure you have installed:
- .NET 8 SDK
- Node.js (v18 or higher)
- SQL Server (LocalDB or full instance)

### Setting Up the Backend

1. Open a terminal and navigate to the Backend folder:
   ```
   cd Backend
   ```

2. Restore the .NET tools (needed for Entity Framework migrations):
   ```
   dotnet tool restore
   ```

3. Restore NuGet packages:
   ```
   dotnet restore
   ```

4. Configure your environment by copying the example config and updating values:
   ```
   cp appsettings.Example.json appsettings.json
   ```
   
   Then edit `appsettings.json` and update these settings:
   
   | Setting | Description |
   | ------- | ----------- |
   | `ConnectionStrings:DefaultConnection` | Your SQL Server connection string. Replace `YOUR_SERVER_NAME` with your server (use `(localdb)\MSSQLLocalDB` for LocalDB) |
   | `Jwt:Key` | A secret key for JWT tokens. Must be at least 32 characters long |
   | `StripeSettings:SecretKey` | Your Stripe secret key (starts with `sk_test_` for test mode). Get one from the Stripe Dashboard |
   | `StripeSettings:WebhookSecret` | Your Stripe webhook secret (optional, needed for payment confirmations) |

5. Create the database and apply migrations:
   ```
   dotnet ef database update
   ```

6. Run the application:
   ```
   dotnet run
   ```

The API will start at `http://localhost:5000`. Swagger documentation is available at `http://localhost:5000/swagger`.

### Setting Up the Frontend

1. Open a new terminal and navigate to the Frontend folder:
   ```
   cd Frontend
   ```

2. Install npm packages:
   ```
   npm install
   ```

3. Configure your environment by copying the example file:
   ```
   cp .env.example .env
   ```
   
   The default values should work for local development. If your backend runs on a different port, update these in `.env`:
   
   | Setting | Description |
   | ------- | ----------- |
   | `VITE_API_BASE_URL` | Base URL for the backend API (default: `http://localhost:5000`) |
   | `VITE_API_URL` | Full API URL with /api path (default: `http://localhost:5000/api`) |

4. Start the development server:
   ```
   npm run dev
   ```

The frontend will start at `http://localhost:5173`.

---

## Features

### Patient Features

**Doctor Search and Booking**
Patients can browse doctors filtered by specialty, view their profiles including consultation fees and hospital affiliations, and book appointments. The booking system assigns queue numbers so patients know their position for each time slot. After booking, patients can pay through Stripe or pay later.

**Real-time Chat**
Once a patient has an appointment with a doctor, they can chat directly through the platform. Messages are stored and displayed with timestamps, making it easy to discuss medical concerns before or after appointments.

**Medical Records**
Patients can upload their medical documents (PDFs, images) organized by category like Lab Results, Prescriptions, or Imaging. Doctors with appointments can view these records to prepare for consultations.

**Hotel Booking**
The platform lists accessible hotels with different room types (Standard, Deluxe, Suite, Family). Each hotel shows amenities, accessibility features, and pricing. Patients can book rooms for their medical trip and pay through Stripe.

**Transport Booking**
Patients can arrange transportation with various vehicle types. The system calculates pricing based on hourly rates and allows special requests for accessibility needs.

**Dashboard**
The user dashboard shows all upcoming appointments with queue positions, hotel bookings, and transport reservations. Patients can manage, pay for, or cancel their bookings from one place.

### Doctor Features

**Appointment Management**
Doctors see all their appointments with patient details, queue numbers, and appointment status. They can accept pending appointments, mark them as completed, and add clinical notes.

**Patient Records Access**
When viewing appointment details, doctors can access the patient's uploaded medical records to review before consultations.

**Profile Management**
Doctors manage their professional profile including specialty, hospital affiliation, consultation fees, and availability.

### Hotel Owner Features

**Hotel Submission**
Hotel owners register and submit their properties for approval. They provide details like location, room types with pricing, amenities (WiFi, Pool, Gym, Restaurant), and accessibility features (wheelchair access, accessible bathrooms, elevator). Amenities are required when submitting a hotel.

**Hotel Management**
After admin approval, hotel owners can edit their listings, update pricing, add images, and view booking statistics on their dashboard.

**Reservation Management**
Hotel owners can view all reservations for their approved hotels in a dedicated Reservations tab. When guests book a room, the reservation appears as "Pending" and the hotel owner must accept or reject it - similar to how doctors approve appointments. The dashboard shows:
- Booking counts per hotel (total, pending, confirmed, rejected)
- Guest details including name, email, and phone number
- Check-in/check-out dates, room type, and number of guests
- Payment status and total price
- Accept/Reject buttons for pending reservations

### Admin Features

**User Management**
Admins can view all registered users, doctors, and hotel owners in the system.

**Hotel Approval Workflow**
When hotel owners submit properties, admins review the details and either approve or reject them. Approved hotels appear in the public listings. Rejected hotels can be edited and resubmitted.

**Transport Management**
Admins manage the transport options available on the platform.

---

## How It Works

### Authentication System
The platform uses separate identity stores for different user types. Regular users, doctors, hotel owners, and admins each have their own registration and login flows. JWT tokens include role claims that the frontend uses to show appropriate navigation and features.

### Booking and Queue System
When a patient books an appointment, the system counts existing bookings for that doctor in the same hour slot and assigns the next queue number. Both patients and doctors see this queue position so everyone knows the order for that time slot.

### Payment Flow
Bookings start as unpaid. When users click "Pay Now", they're redirected to a Stripe checkout session. After successful payment, a webhook updates the booking status. Users can also book now and pay later.

### Hotel Approval Process
Hotel owners submit properties that start with "Pending" status. Admins see these in their dashboard and can approve or reject with comments. Only approved hotels appear in public search results.

### Hotel Booking Approval
When patients book a hotel room, the reservation starts with "Pending" status. Hotel owners see these pending reservations in their dashboard and must accept or reject them. This mirrors the doctor appointment approval flow, giving hotel owners control over their bookings. Owners can filter reservations by hotel and by status (Pending, Confirmed, Rejected).

### File Uploads
Medical records and hotel images are uploaded to the server's wwwroot/uploads folder. The API serves these files statically, and the frontend displays them in carousels or download links.

---

## Application Routes

### Public Pages

| Route | Description |
| ----- | ----------- |
| `/` | Home page |
| `/login` | User login |
| `/register` | User registration |

### User Pages (Requires Login)

| Route | Description |
| ----- | ----------- |
| `/dashboard` | User dashboard with bookings overview |
| `/profile` | User profile management |
| `/doctors` | Browse and search doctors |
| `/book-appointment` | Book a doctor appointment |
| `/hotels` | Browse and search hotels |
| `/book-hotel` | Book a hotel room |
| `/transport` | Browse transport options |
| `/book-transport` | Book transportation |
| `/chat` | Chat with doctors |
| `/medical-records` | View medical records |

### Doctor Portal

| Route | Description |
| ----- | ----------- |
| `/doctor/login` | Doctor login |
| `/doctor/register` | Doctor registration |
| `/doctor/dashboard` | Doctor dashboard |
| `/doctor/profile` | Doctor profile management |
| `/doctor/chat` | Chat with patients |

### Hotel Owner Portal

| Route | Description |
| ----- | ----------- |
| `/hotel/login` | Hotel owner login |
| `/hotel/register` | Hotel owner registration |
| `/hotel/dashboard` | Hotel owner dashboard |

### Admin Portal

| Route | Description |
| ----- | ----------- |
| `/admin/login` | Admin login |
| `/admin/dashboard` | Admin dashboard |

---

## Default Test Accounts

The database seeds with test accounts you can use to explore the platform:

### Admin Account
- Email: `admin@nilecare.com`
- Password: `Admin123!`

### Test Patient Account
- Email: `stefan.mueller@email.com`
- Password: `Password123!`

### Test Doctor Account
- Email: `dr.ahmed@nileheart.com`
- Password: `Doctor123!`

### Test Hotel Owner Account
- Email: `hotel@nilehotels.com`
- Password: `Hotel123!`

---

## API Documentation

For detailed API endpoints, request/response formats, and testing, visit the Swagger documentation at:

```
http://localhost:5000/swagger
```

Swagger provides interactive documentation where you can test all endpoints directly. Use the "Authorize" button with a JWT token to test authenticated endpoints.
