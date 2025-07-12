# 📦 Product Slides Server

This is a lightweight **ASP.NET Core API** that serves static **images** and **videos** for pharmaceutical product slides and automatically **seeds metadata into a SQL Server database** on startup.

## 🧱 Tech Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **Static file serving**

## 📁 Folder Structure

Place your product media inside `wwwroot/products/` like this:

```
wwwroot/
└── products/
    └── Alpraz/
        └── slides/
            ├── images/
            │   ├── 1.jpg
            │   └── 2.jpg
            └── videos/
                ├── 3.mp4
                └── 4.mp4
```

✅ Files must be named with numeric order like `1.jpg`, `2.mp4`, etc.

## 🚀 Getting Started

### 1. ⚙️ Configure SQL Server

Update your `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=productslides;user=root;password=yourpassword;"
}
```

### 2. 📦 Install & Run

```bash
dotnet restore
dotnet ef database update
dotnet run
```

On first run, the server will:
- Create the database (if missing)
- Parse then Seed products and slides from `wwwroot/products/**`
- Serve all files and expose the API

## 📥 Adding New Products

To add a new product:
1. Create a folder under `wwwroot/products/` using the product name.
2. Inside it, place numbered image/video files in `slides/images` and `slides/videos`.
3. Run or restart the app: `dotnet run`

✅ The product and its slides will be auto-seeded to the database.

## 🌐 API Endpoints

### 🔹 Get all products with slides

```
GET /api/products
```

### 🔹 Get slides by product ID

```
GET /api/products/{id}/slides
```

## 🖼️ Static File Access

Access media directly from:

```
http://localhost:{PORT}/products/[ProductName]/slides/images/1.jpg
http://localhost:{PORT}/products/[ProductName]/slides/videos/1.mp4
```

## 🔧 Developer Notes

- Seed data avoids duplicate entries on restart.
- Slide ordering is based on file names (`1.jpg`, `2.mp4`, etc.).

## 👨‍💻 Made by Mouad Halli
