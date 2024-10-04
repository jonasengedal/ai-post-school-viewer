<template>
  <div ref="map" id="map" style="height: 500px;"></div>
  <table>
    <tr>
      <th>Navn</th>
      <th v-for="skole in visibleEfterskoler" :key="skole.id" @mouseover="highlightMarker(skole)"
        @mouseleave="resetHighlightMarker(skole)">{{ skole.navn }} <button @click="removeFromComparison(skole.id)">Fjern</button></th>
    </tr>
    <tr>
      <th>Type</th>
      <td v-for="skole in visibleEfterskoler" :key="skole.id" @mouseover="highlightMarker(skole)"
        @mouseleave="resetHighlightMarker(skole)">{{ skole.type }}</td>
    </tr>
    <tr>
      <th>Beskrivelse</th>
      <td v-for="skole in visibleEfterskoler" :key="skole.id" @mouseover="highlightMarker(skole)"
        @mouseleave="resetHighlightMarker(skole)">{{ skole.kortBeskrivelse }}</td>
    </tr>
  </table>
  <button @click="clearComparison">Ryd sammenligning</button>
  <button @click="showPostSchools">Vis alle skoler</button>
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
    this.efterskoler = JSON.parse(localStorage.getItem('comparisonList')) || [];
    this.visibleEfterskoler = this.efterskoler;
    this.addMarkers();
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
    showPostSchools() {
      this.$router.push('/post-schools');
    },
    clearComparison() {
      localStorage.setItem('comparisonList', "[]");
      this.$router.push('/post-schools');
    },
    removeFromComparison(id) {
      console.log("Fjern " + id);
      this.visibleEfterskoler = this.visibleEfterskoler.filter(i => i.id != id);
      localStorage.setItem('comparisonList', JSON.stringify(this.visibleEfterskoler));
    }
  }
};
</script>
