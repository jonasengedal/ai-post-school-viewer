Sure, I'll guide you through the steps to create this application. First, let's create the scripts and then we'll look at the Vue App and .NET API. 

### Step 1: Install Node.js and Vue CLI
Ensure you have Node.js installed.
Use the following command to install the Vue CLI:
``` bash
npm install -g @vue/cli
```

### Step 2: Vue CLI Script to Setup Vue Project
Create a bash script named `setup_vue.sh`:
```bash
#!/bin/bash
vue create efterskoler-app
cd efterskoler-app
vue add router
vue add vuex
vue add bootstrap-vue
```

### Step 3: Vue CLI Script to Run Vue App
Create a bash script named `run_vue.sh`:
```bash
#!/bin/bash
cd efterskoler-app
npm run serve
```

### Step 4: .NET API Setup
Assuming you have .NET installed, create a bash script for setting up the .NET API:
```bash
#!/bin/bash
dotnet new webapi -n EfterskolerApi
cd EfterskolerApi
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### Step 5: .NET API Run Script
Create a bash script named `run_api.sh`:
```bash
#!/bin/bash
cd EfterskolerApi
dotnet run
```

### Step 6: Creating .NET Efterskole Class
In your .NET project, define the `Efterskole` model:
```csharp
public class Efterskole
{
    public int Id { get; set; }
    public string Navn { get; set; }
    public string KortBeskrivelse { get; set; }
    public string LangBeskrivelse { get; set; }
    public string GpsKoordinater { get; set; }
    public string Adresse { get; set; }
    public List<string> Linjefag { get; set; }
    public List<string> Valgfag { get; set; }
    public List<string> ObligatoriskeFag { get; set; }
    public string Type { get; set; }
}
```

### Step 7: Vue Application Creation
In your Vue App:

1. **Install Leaflet (Map Library)**: Use npm to install it
   ```bash
   npm install leaflet vue2-leaflet
   ```
2. **Set up the Efterskoler Component**:
   Here you would create a Vue component that uses Leaflet for the map and Bootstrap Vue for your table and other UI elements.

3. **Fetch Data from .NET API**:
   Use Axios to fetch data from your .NET API. This will be used to populate the map and the table in your Vue component.

4. **Comparison Storage**:
   Use `localStorage` in JavaScript to handle the "Sammenlign" functionality where you keep track of selected "efterskoler".

5. **Comparison View**:
   Create another Vue component to compare the selected "efterskoler". This will fetch data from `localStorage`.

### Note:
These setups assume you have a working knowledge of bash, Vue.js, and .NET. Also, ensure that you have necessary permissions to execute bash scripts on your machine. Be sure to have all node and .NET dependencies installed before running your scripts.