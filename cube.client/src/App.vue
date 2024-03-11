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
  main {
    display: flex;
    justify-content: center;
    margin-left: auto;
    margin-right: auto;
    margin-top: 50px;
  }
</style>
