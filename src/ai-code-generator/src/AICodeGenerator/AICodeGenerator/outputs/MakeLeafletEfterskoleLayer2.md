Sure! Below is a Vue component that adds a Leaflet point layer on a map by fetching the data from an API. It uses Leaflet to display the map and the points.

```vue
<template>
  <div id="map" style="height: 500px;"></div>
</template>

<script>
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';

export default {
  name: 'LeafletMap',
  mounted() {
    this.initMap();
    this.fetchData();
  },
  methods: {
    initMap() {
      this.map = L.map('map').setView([51.505, -0.09], 13);

      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '© OpenStreetMap contributors'
      }).addTo(this.map);
    },
    fetchData() {
      fetch('YOUR_API_URL')
        .then(response => response.json())
        .then(data => {
          this.addPoints(data);
        })
        .catch(error => console.error('Error loading the data: ', error));
    },
    addPoints(data) {
      data.forEach(item => {
        const coordinates = item.GpsKoordinater;
        const latLng = [coordinates.Lat, coordinates.Long];
        L.marker(latLng).addTo(this.map)
          .bindPopup('Latitude: ' + coordinates.Lat + '<br>Longitude: ' + coordinates.Long);
      });
    }
  }
}
</script>

<style>
/* Make sure the map container has a defined height, here it's set to 500px */
#map {
  height: 500px;
}
</style>
```

Replace `YOUR_API_URL` with the actual URL to your endpoint that returns the JSON data.

Here’s what the code does:
1. The `initMap` method initializes a Leaflet map and adds a tile layer provided by OpenStreetMap.
2. The `fetchData` method fetches data from the API, converts it into JSON, and then calls `addPoints`.
3. The `addPoints` method loops through the returned data and creates markers based on the coordinates.

Make sure you have installed the `leaflet` library (`npm install leaflet`) and have leaflet's CSS file included in your project to see the map properly styled.