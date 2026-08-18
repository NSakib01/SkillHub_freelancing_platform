# SkillHub – Freelance Marketplace Management System

## Project Overview

**SkillHub** is a planned desktop-based freelance marketplace management system for software-development and technology-related services. The platform is designed to connect clients with freelancers while providing an organized workflow for service discovery, ordering, simulated payment, project delivery, reviews, disputes, earnings, and platform administration.

The project is being designed as a **C# WinForms desktop application with SQL Server database integration** for the Object Oriented Programming 2 (OOP2) course.

---

## Project Domain

SkillHub mainly focuses on software and technology services such as:

- Full-Stack Development
- Frontend Development
- Backend Development
- C# / .NET Desktop Development
- Android Application Development
- iOS Application Development
- Rust / Firmware Development
- QA / Software Testing
- Data Analysis
- Cybersecurity
- UI/UX Design
- Graphic Design

---

## Case Story

Nabila Rahman is a small business owner who needs an inventory management application. Instead of searching through scattered social-media posts and unreliable portfolios, she uses SkillHub to search for a suitable C# desktop application developer.

Rafi Ahmed is a freelance software developer who offers C# and .NET development services through SkillHub. Nabila reviews his service details, previous projects, price, rating, and reviews before placing an order.

After receiving the order, Rafi accepts the project and starts development. Nabila can track the progress while Rafi updates the service status. Once the project is finished, Rafi submits the delivery and Nabila can approve it, leave a rating and review, or open a dispute if there is a serious problem.

SkillHub records the simulated payment, deducts the configured platform commission, and records the remaining amount as the freelancer's earnings.

---

## User Roles

The system contains three major user roles.

### 1. SUPER_ADMIN

The SUPER_ADMIN represents the platform owner and can:

- View platform statistics for users, services, orders, payments, disputes, and commission earnings
- Approve, reject, suspend, reactivate, or search freelancer/admin accounts
- Manage service categories
- Manage discount offers and special packages
- Review inappropriate services or reviews
- Control simulated payments
- Handle bad-service disputes
- Handle freelancer withdrawal requests
- View freelancer performance and platform revenue reports

### 2. ADMIN / Freelancer

The ADMIN role represents a freelancer or service provider and can:

- Create and update a professional freelancer profile
- Add skills, biography, and professional information
- Create, read, update, and deactivate service listings
- Set service price, delivery time, and available order slots
- Receive and accept customer orders
- Mark orders as In Progress
- Submit project delivery
- View order and delivery status
- View commission deductions and net earnings
- View wallet transactions
- Submit withdrawal requests
- View ratings and reviews from completed orders

### 3. CUSTOMER / Client

The CUSTOMER role represents a client and can:

- Sign up and sign in
- Browse active services
- Search services by keyword
- Filter services by category, price, rating, delivery time, and availability
- View freelancer information and service details
- View verified reviews
- View special packages and discounts
- Add services to a cart
- Update quantity or remove cart items
- Proceed to checkout
- Use the simulated payment system
- Receive an invoice record
- Track service/order status
- Submit ratings and reviews
- Open a bad-service dispute for eligible orders

---

## Core Features

- Sign Up
- Sign In
- Logout
- Role-based dashboard routing
- User profile management
- Password change
- Multiple user types
- Freelancer profile management
- Service CRUD
- Category management
- Search and filtering
- Cart management
- Checkout
- Simulated payment system
- Platform commission
- Order placement
- Order tracking
- Delivery-status tracking
- Discount offers
- Special packages
- Reviews and ratings
- Dispute management
- Freelancer wallet
- Withdrawal requests
- Admin revenue reports
- Freelancer performance reports

---

## Order Status Flow

A typical service order follows this flow:

`Pending Payment → Placed → In Progress → Delivered → Completed`

Other possible states include:

- Disputed
- Cancelled
- Refunded

---

## Database Design

The planned SQL Server database is normalized to **Third Normal Form (3NF)**.

### Main Tables

1. `Users`
2. `FreelancerProfiles`
3. `Categories`
4. `Services`
5. `Offers`
6. `Carts`
7. `CartItems`
8. `Orders`
9. `OrderItems`
10. `Payments`
11. `Reviews`
12. `Disputes`
13. `WalletTransactions`
14. `WithdrawalRequests`
15. `PlatformSettings`

The report also contains:

- ER Diagram
- Physical Database Schema Diagram
- Primary Key / Foreign Key relationships
- Data dictionary
- Database normalization
- SQL Server table-creation queries
- Sample data
- Feature queries

---

## Important Database Relationships

- A user may have a freelancer profile
- A freelancer can create multiple services
- A category can contain multiple services
- A customer has a cart
- A cart contains multiple cart items
- A customer can place multiple orders
- Orders contain one or more order items
- An order can have a payment record
- An eligible completed order can have a review
- An eligible order can have a dispute
- Freelancer earnings are stored through wallet transactions
- Withdrawal requests are associated with freelancer accounts

---

## Planned Technology Stack

- **Language:** C#
- **Framework:** .NET / Windows Forms
- **Database:** Microsoft SQL Server
- **Database Access:** ADO.NET
- **IDE:** Microsoft Visual Studio
- **Database Tool:** SQL Server Management Studio (SSMS)
- **Version Control:** Git and GitHub

---

## Planned UI / Forms

The report includes planned WinForms wireframes and a navigation flow for:

- Login
- Registration
- Profile / Change Password
- SUPER_ADMIN Dashboard
- User and Freelancer Approval Management
- Platform Sales / Payment / Dispute Report
- Category and Offer Management
- Freelancer Dashboard
- Freelancer Service CRUD
- Freelancer Order and Delivery Status
- Earnings / Wallet / Withdrawal
- Customer Home
- Service Details and Reviews
- Cart
- Checkout / Payment / Invoice
- Order History / Tracking

---

## Project Screenshots

The following screenshots and diagrams are stored in the repository under the `Extras` folder.

| Screenshot 01 | Screenshot 02 |
|---|---|
| ![SkillHub Screenshot 01](Extras/picturesample00001.png) | ![SkillHub Screenshot 02](Extras/picturesample00002.png) |
| ![SkillHub Screenshot 03](Extras/picturesample00003.png) | ![SkillHub Screenshot 04](Extras/picturesample00004.png) |
| ![SkillHub Screenshot 05](Extras/picturesample00005.png) | ![SkillHub Screenshot 06](Extras/picturesample00006.png) |
| ![SkillHub Screenshot 07](Extras/picturesample00007.png) | ![SkillHub Screenshot 08](Extras/picturesample00008.png) |
| ![SkillHub Screenshot 09](Extras/picturesample00009.png) | ![SkillHub Screenshot 10](Extras/picturesample00010.png) |
| ![SkillHub Screenshot 11](Extras/picturesample00011.png) | ![SkillHub Screenshot 12](Extras/picturesample00012.png) |
| ![SkillHub Screenshot 13](Extras/picturesample00013.png) | ![SkillHub Screenshot 14](Extras/picturesample00014.png) |
| ![SkillHub Screenshot 15](Extras/picturesample00015.png) | ![SkillHub Screenshot 16](Extras/picturesample00016.png) |
| ![SkillHub Screenshot 17](Extras/picturesample00017.png) |  |

## Team Members

| Serial | Name | Student ID | Main Contribution |
|---|---|---|---|
| 01 | MD. Nazmus Sakib | 24-58148-2 | Database foundation, schema, SQL planning and integration |
| 02 | Md. Omar Faruk Aumi | 24-59497-3 | Admin and financial module |
| 03 | Sadman Ahmed | 25-62451-2 | Freelancer module |
| 04 | Anika Sumaiya | 24-59063-3 | Client / Customer module |

> Contribution percentages should be entered according to the team's actual agreed workload.

---

## Current Project Status

This repository currently documents the **design and report stage** of SkillHub.

The report provides the complete project blueprint, including the planned database, SQL queries, role-based functionality, UI navigation, and WinForms screen designs. The current report does **not** claim that the full application, real online payment gateway, or complete production system has already been implemented.

---

## Course Information

- **Course:** CSC2210 – Object Oriented Programming 2
- **Section:** D
- **Project:** SkillHub – Freelance Marketplace Management System
- **Institution:** American International University–Bangladesh (AIUB)

---

## Future Implementation

The next development stage will include:

- Creating the WinForms solution
- Building the SQL Server database
- Connecting the application using ADO.NET
- Implementing role-based authentication
- Implementing CRUD operations
- Implementing cart and checkout
- Implementing simulated payment and commission logic
- Implementing order tracking and delivery updates
- Implementing reviews and disputes
- Implementing wallet and withdrawal logic
- Adding validation and exception handling
- Testing all major workflows

---

## Disclaimer

SkillHub is an academic desktop application project. Payment functionality described in the report is simulated for educational purposes and does not represent a real banking or payment-gateway integration.
