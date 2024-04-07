<template>
  <Toast/>
  <header>
    <MyHeader />
  </header>

  <main>
    <router-view></router-view>
  </main>
</template>

<script setup lang="ts">
import MyHeader from "@/components/MyHeader.vue"
import { inject, onMounted } from "vue";
import Toast from "vue3-toastify";
import type { INotificationStore } from "./store/notification.store";
const notificationStore = inject<INotificationStore>('notificationStore') as INotificationStore;


onMounted(async() => {
  setInterval(notificationStore.updateNotificationData, 10000);
});

</script>

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
