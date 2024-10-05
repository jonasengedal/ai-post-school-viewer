<script setup>
import BSTooltip from '@/components/BSTooltip.vue';
import BSNavbar from '@/components/BSNavbar.vue';
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
