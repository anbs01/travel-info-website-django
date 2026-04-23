/**
 * 地区选择弹窗 Alpine.js 组件工厂
 * 用法：x-data="geoPicker('FieldName', currentId, 'currentName')"
 */
function geoPicker(fieldName, initId, initName) {
    return {
        open: false,
        tab: 'domestic',
        loading: false,
        loadingSub: false,
        provinces: [],
        overseasCities: [],
        activeProvince: null,
        subGroups: [],
        selectedId: initId || null,
        selectedName: initName || '',

        async init() {
            this.loading = true;
            try {
                const res = await fetch('/Admin/Api/GeoProvinces');
                const data = await res.json();
                this.provinces = data.domestic;
                this.overseasCities = data.overseas;
            } catch(e) {
                console.error('GeoProvinces load failed', e);
            } finally {
                this.loading = false;
            }
        },

        async selectProvince(p) {
            if (this.activeProvince?.id === p.id) return;
            this.activeProvince = p;
            this.subGroups = [];
            this.loadingSub = true;
            try {
                const res = await fetch(`/Admin/Api/GeoByProvince?provinceId=${p.id}`);
                this.subGroups = await res.json();
            } catch(e) {
                console.error('GeoByProvince load failed', e);
            } finally {
                this.loadingSub = false;
            }
        },

        select(id, name) {
            this.selectedId = id;
            this.selectedName = name;
            this.open = false;
        },

        clear() {
            this.selectedId = null;
            this.selectedName = '';
            this.activeProvince = null;
            this.subGroups = [];
        }
    };
}
