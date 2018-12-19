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
                    <button @click="setColor" class="button is-primary is-large is-fullwidth">
                        Apply
                    </button>
                </div>
            </div>
            <div class="columns is-centered">
                <div class="column is-half">
                    {{statusMessage}}
                </div>
            </div>
            <div class="columns is-centered">
                <!-- <video-player class="video-player-box"
                    ref="videoPlayer"
                    :options="playerOptions"
                    :playsinline="true">
                </video-player> -->
                <img controls src="http://192.168.1.157:8080/stream/video.mjpeg"/>
            </div>
        </div>
    </section>
</div>
</template>
<script lang="ts">
import Vue from 'vue';
import { Chrome } from 'vue-color';
import LedService from '../services/led.service';
import Component from 'vue-class-component';
import { videoPlayer } from 'vue-video-player';
@Component({
    components: {
        'chrome-picker': Chrome,
        videoPlayer,
    },
})
export default class Home extends Vue {
    public playerOptions = {
        sources: [{
            type: 'video/mp4',
            src: 'http://192.168.1.157:8090/',
        }],
    };
    public ledService = new LedService();
    public color =  { r: 255, g: 0, b: 0 };
    public displayColor: string = '';
    public statusMessage: string = '';
    public updateValue(val: any): void {
        this.color = val.rgba;
        this.displayColor = val.rgba;
    }
    public async setColor() {
        try {
            await this.ledService.setLedColors(Number(this.color.g), Number(this.color.b), Number(this.color.r));
            this.statusMessage = 'Success!';
        } catch (ex) {
            this.statusMessage = 'Error :(';
        }
    }
}
</script>

