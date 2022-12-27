<script lang="ts">
import Vue, {CreateElement, VNode} from 'vue';
import { mapMutations } from "vuex";
import {ReceivedDepartment} from "@/store/incoming_types";
import {NormalizedScopedSlot} from "vue/types/vnode";

export default Vue.extend({
  name: "async_departments",
  props: {
    request: { type: Function, required: true },
    params: { type: Object, default: () => ({}) }
  },
  data() {
    return {
      pending: true,
      error: false as boolean | Error
    };
  },
  watch: {
    url() {
      this.requestData();
    },
    params: {
      handler() {
        this.requestData();
      },
      deep: true
    }
  },
  mounted() {
    this.requestData();
  },
  methods: {
    ...mapMutations([
      "resetDepartments",
    ]),
    async requestData() {
      this.pending = true;

      this.request()
          .then((response: Response) => {
            if (!response.ok) {
              throw new Error("")
            }

            return response.json();
          })
          .then((data: ReceivedDepartment[]) => {
            this.resetDepartments(data);
            this.error = false;
          })
          .catch((e: Error) => {
            this.error = e;
          })
          .finally(() => {
            this.pending = false;
          })
    }
  },
  render(createElement: CreateElement): VNode {
    return createElement('div', (this.$scopedSlots as { [key: string]: NormalizedScopedSlot }).default({
      pending: this.pending,
      error: this.error
    }));
  }
});
</script>
