# SkillHub Refinement Verification Checklist

Use this checklist after running `Database/SkillHubDatabase.sql` and
`Database/SkillHub_Verification.sql` on the target Windows computer.

## Automated package checks completed

- The Visual Studio project XML parses successfully.
- Every explicitly referenced source, resource, document and image file exists.
- All 80 C# compilation units have balanced lexical structures and no merge markers.
- All SQL image paths resolve to the 15 bundled local assets.
- The SQL seed contains six freelancers, four clients and thirteen unique services.
- The three documented demonstration-password hashes were independently verified.
- The complete portrait and service-artwork contact sheets were visually inspected.

## Windows build and runtime smoke test

1. Open `SkillHub.sln` in Visual Studio 2022 and select **Build > Rebuild Solution**.
2. Run `Database/SkillHubDatabase.sql`, followed by
   `Database/SkillHub_Verification.sql`; confirm all `PASS` messages.
3. Sign in as `client@skillhub.local` with `Client@123`.
4. Open the marketplace and verify all thirteen service cards render with images.
5. Search for `security`, filter by category and exercise every sorting option.
6. Open a service, confirm the freelancer profile and full description appear,
   then add it to the cart and complete the simulated checkout.
7. Sign in as `freelancer@skillhub.local` with `Freelancer@123`.
8. Update the freelancer photo and profile, then create and update a service image.
9. Confirm the new or updated listing appears in the client marketplace.
10. Sign in as `admin@skillhub.local` with `Admin@123` and verify the platform
    administration, moderation and reporting screens still open normally.

The application targets .NET Framework 4.8 and SQL Server Express. If the local
SQL instance is not named `.\SQLEXPRESS`, change only `Data Source` in `App.config`.
