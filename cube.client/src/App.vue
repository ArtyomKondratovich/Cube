<script setup lang="ts">
import MyHeader from "@/components/MyHeader.vue"
import Toast from "vue3-toastify";
</script>

<template>
  <Toast/>
  <header>
    <MyHeader />
  </header>

  <main>
    <router-view></router-view>
  </main>
</template>

<style>

</style>
