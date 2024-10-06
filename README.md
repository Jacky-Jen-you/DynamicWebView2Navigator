```markdown
# DynamicWebView2Navigator

DynamicWebView2Navigator is a dynamic web navigation application built using WebView2. It dynamically loads navigation content and enables switching between pages. The app's main interface is split into two sections: a Navigator on the left and the content display on the right. The navigation items are loaded from a configuration file located under the `Config\Navigations` directory.

## Features

- **Dynamic Navigation**: Loads navigation items from external configuration files, including HTML file paths, display names, and backend logic handlers (WebBridge classes).
- **Frontend-Backend Communication**: Each navigation item is linked to a WebBridge class that handles data exchange between the frontend and backend. These WebBridge classes inherit from `WebBridgeBase`.
- **IoC Container**: WebBridge instances are registered in an IoC container to ensure each page's logic is uniquely managed.
- **Page Switching**: When a user selects a navigation item, the corresponding page is loaded in the right-side WebView2, and the relevant WebBridge instance is retrieved from the IoC container to handle the page's logic and data exchange.
- **Scalable**: Easily extendable by adding new navigation settings and corresponding WebBridge classes for new pages.

## How It Works

1. **Loading Navigation Items**: On startup, the app loads navigation items from the `Config\Navigations` directory. Each item includes:
   - HTML file path
   - Display label
   - Corresponding WebBridge class for handling frontend-backend communication.

2. **WebBridge**: Each navigation item has a corresponding WebBridge instance, which is responsible for data exchange between the frontend (HTML) and the backend (logic in C#).

3. **Dynamic Page Loading**: When the user selects a navigation item, the app dynamically loads the HTML file into the WebView2 on the right side, and the relevant WebBridge instance is loaded from the IoC container to handle page-specific logic.

## Project Structure

```
WebView2Demo/
 ¢x
¢u¢w¢w Config/
¢x   ¢|¢w¢w Navigations/              # Contains the configuration files for navigation items
¢x
¢u¢w¢w WebBridges/                   # Contains the WebBridge classes for frontend-backend communication
¢x   ¢u¢w¢w Pages/                    # All custom WebBridge implementations
¢x   ¢|¢w¢w WebBridgeBase.cs          # Base class for all WebBridge implementations
¢x
¢u¢w¢w Assets/                       # Assets for the project, including custom views and resources
¢x   ¢|¢w¢w Pages/                    # Each page has its own folder for related resources (HTML, JS, CSS)
¢x       ¢u¢w¢w Page1/                # Folder for page "Page1"
¢x       ¢x   ¢u¢w¢w index.html        # HTML file for Page1
¢x       ¢x   ¢u¢w¢w script.js         # JavaScript file for Page1
¢x       ¢x   ¢|¢w¢w styles.css        # CSS file for Page1
¢x       ¢u¢w¢w Page2/                # Folder for page "Page2"
¢x       ¢x   ¢u¢w¢w index.html        # HTML file for Page2
¢x       ¢x   ¢u¢w¢w script.js         # JavaScript file for Page2
¢x       ¢x   ¢|¢w¢w styles.css        # CSS file for Page2
¢x       ¢|¢w¢w ...                   # More pages as needed
```

## How to Add a New Page

1. Add a new HTML file in the `Views/` directory for the page content.
2. Update the `Config\Navigations` directory with a new configuration file for the navigation item, specifying:
   - The HTML file path
   - The display label for the navigation
   - The WebBridge class responsible for the page.
3. Create a new WebBridge class (if necessary) to handle logic and data exchange for the new page.

## Requirements

- WebView2 Runtime
- .NET 6.0 or later

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
```¡C