# Simple service to show concepts about parameters and business rules

## How to create user in sql

```sql
use Northwind
CREATE LOGIN NWProductReader WITH PASSWORD = '123abcABC1244';
CREATE USER NWProductReader FOR LOGIN NWProductReader; 

GRANT SELECT ON dbo.products to [NWProductReader]


select sp.name as login,
       sp.type_desc as login_type
      
from sys.server_principals sp
where sp.name = 'NWProductReader'

ALTER LOGIN [NWProductReader] WITH PASSWORD = '123abcABC1245';
```