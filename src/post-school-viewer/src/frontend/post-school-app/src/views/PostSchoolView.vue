<template>
  <div ref="map" id="map" style="height: 500px;"></div>
  <table>
    <tr>
      <th>Navn</th>
      <th>Type</th>
      <th>Beskrivelse</th>
      <th></th>
    </tr>
    <tr v-for="skole in visibleEfterskoler" :key="skole.id"
      @mouseover="highlightMarker(skole)" @mouseleave="resetHighlightMarker(skole)">
      <td>{{ skole.navn }}</td>
      <td>{{ skole.type }}</td>
      <td>{{ skole.kortBeskrivelse }}</td>
      <td><button @click="addToComparison(skole)">Sammenlign</button></td>
    </tr>
  </table>
  <button @click="showComparison">Vis Sammenligning</button>
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
