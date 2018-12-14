<style lang="scss">
  @import 'bulma';
</style>
<style scoped>
    .center-picker { margin-left: 33%; }
</style>
<template>
<div>
    <section class="hero is-info is-bold">
        <div class="hero-body has-text-centered">
            <h1 class="title">Set your colors here!</h1>
        </div>
    </section>
    <section class="section">
        <div class="container is-dark">
            <div class="columns is-centered">
                <div class="column is-half has-text-centered">
                    <chrome-picker class="center-picker" v-model="color" @input="updateValue"/>
                </div>
            </div>
            <div class="columns is-centered">
                <div class="column is-half has-text-centered">
                    {{displayColor}}
                </div>
            </div>
            <div class="columns is-centered">
                <div class="column is-half">
                    <button class="button is-primary is-large is-fullwidth">
                        Apply
                    </button>
                </div>
            </div>
        </div>
    </section>
</div>
</template>
<script lang="ts">
import Vue from 'vue';
import { Chrome } from 'vue-color';
import Component from 'vue-class-component';
@Component({
    components: {
        'chrome-picker': Chrome
    }
})
export default class Home extends Vue {
    color =  { r: 255, g: 0, b: 0 };
    displayColor: string = '';
    updateValue(val: any): void {
        this.color = val.rgba;
        this.displayColor = val.rgba;
    }
}
</script>

