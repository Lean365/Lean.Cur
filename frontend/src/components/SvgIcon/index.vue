<template>
  <svg
    :class="['svg-icon', className]"
    :style="{ width: size + 'px', height: size + 'px' }"
    aria-hidden="true"
  >
    <use :xlink:href="'#icon-' + name" />
  </svg>
</template>

<script setup lang="ts">
defineProps({
  name: {
    type: String,
    required: true
  },
  className: {
    type: String,
    default: ''
  },
  size: {
    type: Number,
    default: 16
  }
});
</script>

<style lang="less" scoped>
.svg-icon {
  fill: currentColor;
  vertical-align: middle;
}</style> 