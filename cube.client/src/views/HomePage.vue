<template>
    <div class="main">
        <div>
        <Menu></Menu>
    </div>
    <div class="posts">
        <div v-if="loading">
            <p>Loading posts...</p>
        </div>
        <div v-if="!loading">
            <ul>
                <li>
                </li>
            </ul>
        </div>
    </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import Menu from '@/components/Menu.vue';

export default defineComponent({
    name: 'Home',
    data(){
        return {
            id: 0,
            loading: true,
            posts: []
        }
    },
    components: {
        Menu
    },
    methods: {

    }
});

</script>

<style>
.main {
    display: flex;
    width: 80%;
}

.posts {
    border: 0.5px solid grey;
    border-radius: 15px;
    margin-left: 10px;
    width: 80%;
}


</style>