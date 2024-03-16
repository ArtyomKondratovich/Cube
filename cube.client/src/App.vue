<script lang="ts">
import { defineComponent } from "vue";
import MyHeader from "@/components/MyHeader.vue"
import Toast from "vue3-toastify";

export default defineComponent({
  Name: 'App',
  components: {
    MyHeader,
    Toast
  }
})
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
    color: white;
    width: 100%;
    height: 100%;
  }
  header {
    display: flex;
    justify-content: center;
    width: 100%;
    background-color: #222222;
  }
</style>
