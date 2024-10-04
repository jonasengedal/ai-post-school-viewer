<template>
  <div class="container-fluid">
    <div class="row">
      <div class="col-8">
        <div ref="map" id="map" style="height: 500px;"></div>
      </div>
      <div class="col-4">
        <ul class="list-group">
          <li class="list-group-item" v-for="skole in visibleEfterskoler" :key="skole.id"
            @mouseover="highlightMarker(skole)" @mouseleave="resetHighlightMarker(skole)">
            <h5>{{ skole.navn }} ({{ skole.by }})</h5>
            <p>{{ skole.kortBeskrivelse }}</p>
            <p><b>Type:</b> {{ skole.type }}</p>
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
      fetch('https://localhost:7066/api/efterskoler')
        .then(resp => resp.json())
        .then(data => {
          this.efterskoler = data;
          this.addMarkers();
          this.visibleEfterskoler = this.efterskoler;
        });
    },
    addMarkers() {
      this.efterskoler.forEach(skole => {
        const marker = L.circleMarker([skole.gpsKoordinater.lat, skole.gpsKoordinater.long], {radius: 5}).addTo(this.map)
          .bindPopup(`<strong>${skole.navn}</strong><br>${skole.by}`);
        this.markers.push({ data: skole, marker: marker });
      });
    },
    filterVisibleEfterskoler() {
      const bounds = this.map.getBounds();
      this.visibleEfterskoler = this.efterskoler.filter(skole =>
        bounds.contains([skole.gpsKoordinater.lat, skole.gpsKoordinater.long]));
    },
    highlightMarker(skole) {
      const selected = this.markers.find(m => m.data.id === skole.id);
      selected.marker.openPopup();
    },
    resetHighlightMarker(skole) {
      const selected = this.markers.find(m => m.data.id === skole.id);
      selected.marker.closePopup();
    },
    addToComparison(skole) {
      if (!this.comparisonList.find(el => el.id === skole.id)) {
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
