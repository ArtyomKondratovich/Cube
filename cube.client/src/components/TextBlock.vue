<template>
    <div style="display: flex; width: 100%; height: 100%; justify-content: center; align-items: center;">
        {{ props.message }}
    </div>
</template>

<script setup lang="ts">

const props = defineProps({
    message: {
        type: String,
        required: true
    }
});

</script>

<style>
</style>