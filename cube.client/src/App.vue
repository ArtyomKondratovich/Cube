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
    display: flex;
    justify-content: center;
    margin-left: auto;
    margin-right: auto;
    color: white;
    width: 70%;
    height: 100%;
  }
</style>
