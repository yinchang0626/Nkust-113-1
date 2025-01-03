# **ASP.NET MVC Development Progress**
Authoer: kerong
---

### **2024/12/27: Installing Entity Framework and DB Context**
- **Objective**: Integrating Entity Framework to manage database interactions and setting up the DB Context.
- **Image**: ![image](https://hackmd.io/_uploads/SJrRpNTBJg.png)

#### **Controller Creation**
- **ClubController.cs**: Linked the Index action to display club data.
- **RaceController.cs**: Set up the controller to handle race-related actions and views.

#### **Model Creation**
- Developed models to represent various data entities and user information.
    - **Address.cs**: Model for storing address details.
    - **AppUser.cs**: Model representing the application user.
    - **Club.cs**: Model for representing club details.
    - **Race.cs**: Model for representing race details.

#### **View Creation**
- Created the views for displaying data on the website.
    - **Club/Index.cshtml**: View for displaying the list of clubs.
    - **Race/Index.cshtml**: View for displaying the list of races.

---

### **2024/12/18: Detail Page and Dependency Injection Setup**
- **Objective**: Add a detail page and implement Dependency Injection (DI) and Repository Pattern for clean architecture.
- **Image**: ![image](https://hackmd.io/_uploads/BkQg04TByg.png)
  ![image](https://hackmd.io/_uploads/S1OYGLTH1l.png)
  ![image](https://hackmd.io/_uploads/rJO_Yv6Skl.png)

#### **Layout Configuration**
- Set the layout for shared views, including club and race sections.
    - **Views/Shared/_Layout.cshtml**: Centralized layout for the club and race sections.

#### **Detail Page Creation**
- Created the detail pages for both club and race entities.
    - **Views/Club/Detail.cshtml**: Detail page for viewing specific club information.
    - **Views/Race/Detail.cshtml**: Detail page for viewing specific race information.

#### **Interfaces and Repository**
- **IClubRepository.cs**: Defined the repository interface for accessing club data.
- **ClubRepository.cs**: Implemented the repository to handle data operations related to clubs.

#### **About Page**
- Created an About page to display general information about the application.
    - **Views/Home/About.cshtml**: Page for introducing the application.

---

### **2024/12/29: Club Creation and Cloud Integration**
- **Objective**: Implement functionality to create a club and upload photos to Cloudinary using an API key.
- **Image**: ![image](https://hackmd.io/_uploads/SJu851y8yx.png)  
  ![image](https://hackmd.io/_uploads/HJireKRSyg.png)

#### **Create Club Functionality**
- Created the "Create" button for the club creation feature.
    - **Views/Club/Create.cshtml**: View to allow users to create new clubs.

#### **Controller Refinement**
- Refined the logic in the **ClubController.cs** to handle the new club creation flow.

#### **Service Creation**
- Developed services for handling photo uploads and location-related operations.
    - **PhotoService.cs**: Service to manage photo uploads to Cloudinary.
    - **LocationService.cs**: Service to manage location-related data.

#### **ViewModels**
- **CreateClubViewModel.cs**: ViewModel to handle the data for creating a new club.

#### **Helpers**
- **CloudinarySetting.cs**: Configuration for Cloudinary API settings.
- **IPinfo.cs**: Helper for extracting IP-related information.
- **Location.cs**: Model for handling location details.
- **StateConverter.cs**: Helper for converting between state-related data formats.
