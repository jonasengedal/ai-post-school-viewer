To add a Leaflet point layer that fetches data from an API using Vue.js, you can follow the steps below. Let's assume you want to display the points on a Leaflet map once the Vue component is created, and the API provides data in GeoJSON format.

First, make sure you have Leaflet and Vue.js included in your project. You can install Leaflet through npm or include it via a CDN. For this example, we'll use a CDN for simplicity:

```html
<head>
  <!-- Leaflet CSS -->
  <link rel="stylesheet" href="https://unpkg.com/leaflet@1.7.1/dist/leaflet.css" />

  <!-- Vue.js -->
  <script src="https://unpkg.com/vue@2.6.14/dist/vue.js"></script>
  
  <!-- Leaflet JS (should be included after Vue.js to make sure Vue.js is loaded first) -->
  <script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js"></script>
</head>
```

Next, here is an example of a Vue component for adding a Leaflet map with a point layer from an API:

```html
<div id="app">
  <div id="map" style="height: 500px;"></div>
</div>

<script>
  new Vue({
    el: '#app',
    mounted() {
      this.loadMap();
    },
    methods: {
      loadMap() {
        // Create the map
        var map = L.map('map').setView([51.505, -0.09], 13);

        // Set up the OSM layer
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 19,
          attribution: '© OpenStreetMap'
        }).addTo(map);

        // Fetch data from the API
        fetch('https://api.example.com/data')
          .then(response => response.json())
          .then(data => {
            // Assuming 'data' is a GeoJSON object
            L.geoJSON(data, {
              // Define the pointToLayer function to customize marker icons
              pointToLayer: function (feature, latlng) {
                return L.marker(latlng);
              }
            }).addTo(map);
          })
          .catch(error => console.error('Error loading the API data: ', error));
      }
    }
  });
</script>
```

In this example:

1. The application mounts and calls the `loadMap` method.
2. The `loadMap` method initializes the map and sets the base OpenStreetMap layer.
3. It fetches data from an API which returns GeoJSON, then adds this data as a point layer on the map using `L.geoJSON`.
4. Error handling is added in the fetch function to log issues when fetching data.

Note:
- Ensure you have the necessary permissions and API keys if required by the data API.
- Replace `'https://api.example.com/data'` with the actual URL to fetch data from your API.
- Customize the map's initial view using your preferred latitude and longitude values in `setView`.