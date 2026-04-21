# 📦 Warehouse Management System (WMS) – ASP.NET Core API

## 🚀 Overview

This project represents a simplified and sanitized version of an enterprise-grade Warehouse Management System (WMS) backend.

It is designed to manage inventory, warehouse operations, order processing, and logistics workflows through a modular and scalable API architecture.

> ⚠️ Due to confidentiality, the full production system and sensitive configurations are not included.

---

## 🧩 Core Features

### 📦 Inventory & Storage Management

* Manage items, locations, zones, and storage containers
* Track stock movement and warehouse organization

### 📥 Order Processing

* Handle order creation and order lines
* Manage picking, packing, and dispatch workflows

### 🔄 Warehouse Operations

* PutAway (storing items)
* Picking (order fulfillment)
* Induction and movement handling

### 🖨️ Labeling & Printing

* Package label generation
* Print management for warehouse operations

### ⚙️ System Configuration

* User management
* System preferences
* Control numbers and configurations

---

## 🛠️ Tech Stack

* ASP.NET Core Web API
* C#
* RESTful API architecture
* Layered architecture (Controller → Service → Repository)

---

## 📁 Project Structure

```bash
Controllers/        # API endpoints (Orders, Inventory, Picking, etc.)
Services/           # Business logic layer
Repositories/       # Data access abstraction
DAL/                # Database interaction layer
Models/             # Domain models
```

---

## 📌 Key Modules (Based on Implementation)

* `Orders / OrderLines` → Order management
* `Items / Locations / Zones` → Inventory & storage
* `HotPick / HotPutAway` → Warehouse execution flows
* `Printing / PackageLabels` → Label and print handling
* `Users / SystemPreferences` → Configuration & access

---

## 💡 My Contributions

* Designed and implemented RESTful APIs for warehouse operations
* Built modular architecture for scalability and maintainability
* Developed business workflows for picking, packing, and storage
* Structured backend using layered architecture principles
* Worked with real-world logistics and inventory scenarios

---

## 🔒 Disclaimer

This repository contains **partial and simplified code** for demonstration purposes only.

* Sensitive business logic has been removed
* Database configurations and connection strings are excluded
* Internal integrations and proprietary components are not included
