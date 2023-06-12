# LiteratureApp

## Description

LiteratureApp is a library application developed using C# and WPF. It utilizes Entity Framework (EF) for interacting with the MS SQL database and follows the Model-View-ViewModel (MVVM) design pattern. The application provides a user-friendly interface with animations and elements from the Material Design library for a pleasant user experience.

Upon registration, a standard user profile is created. Users with higher access levels will have access to all menu items. The application features:

1. **Library**: Users can search, filter, and select books.
2. **User Profile**: Displays the user's reading lists and allows profile picture updates. A standard placeholder image is set for newly registered users.
3. **Settings**: Users can update their information and change the application theme.
4. **Admin Panel**: Available only to administrators, it allows adding, editing, and deleting entries from the associated database.

When a book is selected, its page opens with its photo, information, ratings (immediately showing the average and whether the current user has rated it), and statistics on its presence in other users' lists and their ratings. Users can also start reading from a bookmark or from the beginning. 

The reading window is designed for comfort, with settings allowing users to change the theme and font size. Users can also leave comments linked to specific text excerpts. Comments can be made private or public and can be deleted by the users who left them or by moderation. 

## Installation

To install and run the project:

1. Clone the repository to your local machine.
2. Install .NET Framework if not already installed.
3. Open the project in Visual Studio or any supported IDE.
4. Run the project by clicking the "Start" or "Run" button in your IDE.

## Usage

After launching the application, you can register or log in if you already have an account. Once logged in, you can view and manage literary works. 
