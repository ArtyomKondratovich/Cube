<template>
    <div class="messanger">
        <div style="display: flex; flex-basis: 30%;">
            <RouterView name="ChatsBlock"></RouterView>
        </div>
        <div style="display: flex; flex-basis: 70%;">
            <RouterView name="ChatBlock"></RouterView>
        </div>
    </div>
</template>

<script setup lang="ts">

</script>


<style>
    .messanger {
        display: flex;
        width: 100%;
    }

    .selectAnyChat{
        display: flex;
        align-items: center;
        justify-content: center;
        flex-basis: 70%;
    }

</style>