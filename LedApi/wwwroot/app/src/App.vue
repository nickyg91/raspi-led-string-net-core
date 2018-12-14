<style lang="scss">
  @import 'bulma';
</style>
<template>
  <div id="app">
    <!-- <nav class="navbar is-dark">
      <div class="navbar-brand">
        <div class="has-text-white">
          <h1 class="is-size-1">
            Lights!
          </h1>
        </div>
      </div>
      <div class="navbar-menu">
        <div class="navbar-start">
          <router-link class="navbar-item" to="/">Home</router-link>
        </div>
      </div>
    </nav> -->
    <div class="is-dark">
      <router-view />
    </div>
  </div>
</template>
