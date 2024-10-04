Below I provide the Vue application code, and the .NET API code to fetch 'efterskoler'.

Vue Application:

```html
<template>
  <div class="container-fluid">
    <div class="row">
      <div class="col-8">
        <div ref="map" id="map" style="height: 500px;"></div>
      </div>
      <div class="col-4">
        <ul class="list-group">
          <li class="list-group-item" v-for="skole in visibleEfterskoler" :key="skole.Navn"
            @mouseover="highlightMarker(skole)"
            @mouseleave="resetHighlightMarker(skole)">
            <h5>{{ skole.Navn }} ({{ skole.By }})</h5>
            <p>{{ skole.KortBeskrivelse }}</p>
            <p><b>Type:</b> {{ skole.Type }}</p>
            <button @click="addToComparison(skole)" class="btn btn-primary">Sammenlign</button>
          </li>
        </ul>
        <button class="btn btn-success mt-3" @click="showComparison">Vis Sammenligning</button>
      </div>
    </div>
  </div>
</template>

<script>
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';

export default {
  data() {
    return {
      map: null,
      markers: [],
      efterskoler: [],
      visibleEfterskoler: [],
      comparisonList: []
    }
  },
  mounted() {
    this.loadMap();
    this.fetchEfterskoler();
    this.comparisonList = JSON.parse(localStorage.getItem('comparisonList')) || [];
  },
  methods: {
    loadMap() {
      this.map = L.map('map').setView([55.6761, 12.5683], 6);
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(this.map);
      this.map.on('zoomend moveend', () => {
        this.filterVisibleEfterskoler();
      });
      this.map.on('fullscreenchange', () => {
        this.map.invalidateSize();
      });
    },
    fetchEfterskoler() {
      fetch('/api/efterskoler')
        .then(resp => resp.json())
        .then(data => {
          this.efterskoler = data;
          this.addMarkers();
          this.visibleEfterskoler = this.efterskoler;
        });
    },
    addMarkers() {
      this.efterskoler.forEach(skole => {
        const marker = L.marker([skole.GpsKoordinater.lat, skole.GpsKoordinater.lng]).addTo(this.map)
          .bindPopup(`<strong>${skole.Navn}</strong><br>${skole.By}`);
        this.markers.push({ data: skole, marker: marker });
      });
    },
    filterVisibleEfterskoler() {
      const bounds = this.map.getBounds();
      this.visibleEfterskoler = this.efterskoler.filter(skole =>
        bounds.contains([skole.GpsKoordinater.lat, skole.GpsKoordinater.lng]));
    },
    highlightMarker(skole) {
      const selected = this.markers.find(m => m.data.Navn === skole.Navn);
      selected.marker.openPopup();
    },
    resetHighlightMarker(skole) {
      const selected = this.markers.find(m => m.data.Navn === skole.Navn);
      selected.marker.closePopup();
    },
    addToComparison(skole) {
      if (!this.comparisonList.find(el => el.Navn === skole.Navn)) {
        this.comparisonList.push(skole);
        localStorage.setItem('comparisonList', JSON.stringify(this.comparisonList));
      }
    },
    showComparison() {
      this.$router.push('/comparison');
    }
  }
};
</script>

<style scoped>
</style>
```

.NET Core API (`Controllers/EfterskolerController.cs`):

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MyEfterskolerApp.Models; // add your model namespace

namespace MyEfterskolerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EfterskolerController : ControllerBase
    {
        private readonly DataContext _context;

        public EfterskolerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Efterskole>> GetEfterskoler()
        {
            return _context.Efterskoler.ToList();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEfterskole(int id, Efterskole efterskole)
        {
            if (id != efterskole.Id) return BadRequest();

            _context.Entry(efterskole).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
```

Note: Ensure that you have `DataContext` and `Efterskole` entities properly defined in your data model and that CORS policy is configured correctly in your .NET app to allow requests from your Vue app.
Also, you should add Bootstrap CSS in your project's layout for the Bootstrap components to render correctly.