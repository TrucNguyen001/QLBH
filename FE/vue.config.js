const { defineConfig } = require("@vue/cli-service");

module.exports = defineConfig({
  transpileDependencies: true,
  configureWebpack: {
    devtool: "source-map", // Add this line to enable source maps
  },
  devServer: {
    headers: {
      "Cross-Origin-Embedder-Policy": "unsafe-none",
    },
  },
});
