import { createApp } from "vue";
import App from "./App.vue";
import router from "./js/router/router.js";

import MInput from "./componentsweb/base/MInput.vue";
import MDate from "./componentsweb/base/MDate.vue";
import MCheckBox from "./componentsweb/base/MCheckBox.vue";
import MCombobox from "./componentsweb/base/MCombobox.vue";
import MDropDownList from "./componentsweb/base/MDropDownList.vue";
import MPagination from "./componentsweb/base/MPagination.vue";
import MHelper from "./js/helper/helper.js";
import tinyEmitter from "tiny-emitter/instance";
import axios from "axios";
import MApiService from "./js/apiservice/apiservice";
import MCommon from "./js/common/common.js";
import MResource from "./js/helper/resource.js";
import MTextArea from "./componentsweb/base/MTextAra.vue";

import "./js/interceptors/interceptors.js";
import "./js/keyboardevenhandler/keyboardeventhandler.js";

import DatePicker from "vue3-datepicker";

import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";

const app = createApp(App);
app.use(DatePicker);
app.component("DatePicker", DatePicker);

import GAuth from "vue3-google-oauth2";

import CKEditor from "@ckeditor/ckeditor5-vue";

import { gapi } from "gapi-script";
const gAuthOptions = {
  clientId:
    "215795800085-080cimpgt6vp7s1mvejmel3fnjs6iqif.apps.googleusercontent.com",
  scope: "email",
  prompt: "consent",
  fetch_basic_profile: false,
};
app.use(GAuth, gAuthOptions);
app.use(CKEditor);
gapi.load("client: auth2", () => {
  gapi.client.init({
    clientId:
      "215795800085-080cimpgt6vp7s1mvejmel3fnjs6iqif.apps.googleusercontent.com",
    plugin_name: "chat",
  });
});

app.component("MInput", MInput);
app.component("MDate", MDate);
app.component("MCheckBox", MCheckBox);
app.component("MCombobox", MCombobox);
app.component("MPagination", MPagination);
app.component("MDropDownList", MDropDownList);
app.component("MTextArea", MTextArea);
app.config.globalProperties.api = axios;
app.config.globalProperties.emitter = tinyEmitter;
app.config.globalProperties.helper = MHelper;
app.config.globalProperties.resource = MResource;
app.config.globalProperties.apiService = MApiService;
app.config.globalProperties.common = MCommon;

app.use(router).mount("#app");
