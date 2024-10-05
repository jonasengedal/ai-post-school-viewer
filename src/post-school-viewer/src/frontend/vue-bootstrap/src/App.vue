<script setup>
import  { onMounted, inject } from 'vue';
import BSTooltip from '@/components/BSTooltip.vue';
import BSNavbar from '@/components/BSNavbar.vue';

// Inject Bootstrap with the same key as defined in main.js
// const bootstrap = inject('bootstrap');

// onMounted(() => {
//   try {
//     // Expanding the navbar.
//     const collapse = new bootstrap.Collapse('#navbarSupportedContent');
//     collapse.show();
//   } catch (e) {
//     console.log('Bootstrap error: ', e);
//   }
// });
</script>

<template>
  <div id="wrapper">
    <BSNavbar />
    <BSTooltip />
  </div>
</template>

<style scoped>
#wrapper {
  max-width: 1280px;
  margin: 0 auto;
}
</style>
