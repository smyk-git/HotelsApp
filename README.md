# HotelApp – System rezerwacji hoteli (ASP.NET MVC)

Projekt zaliczeniowy wykonany w technologii **ASP.NET MVC**, realizujący system rezerwacji hoteli z obsługą użytkowników oraz administratora.  
Aplikacja została przygotowana zgodnie z wymaganiami specyfikacji projektu ASP.NET MVC.

---

## 1. Wymagania techniczne

- Technologia: **ASP.NET MVC**
- ORM: **Entity Framework Core**
- Baza danych: **SQL (SQL Server / SQLite – zgodnie z konfiguracją)**
- Autoryzacja i logowanie: **ASP.NET Identity**

---

## 2. Instalacja i uruchomienie aplikacji

1. Sklonuj repozytorium z GitHub.
2. Otwórz projekt w Visual Studio.
3. W pliku `appsettings.json` skonfiguruj **connection string** do bazy danych SQL.
4. Wykonaj migracje bazy danych:
   ```bash
   dotnet ef database update
