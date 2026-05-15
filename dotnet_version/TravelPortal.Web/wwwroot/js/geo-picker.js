function geoPicker(fieldName, initId, initName) {
    return {
        open: false,
        tab: 'domestic',
        loading: false,
        searchQuery: '',
        path: [], // 面包屑路径 [{id, title}]
        items: [], // 当前层级的原始数据
        selectedId: initId || null,
        selectedName: initName || '',

        // 初始化加载顶级（省份或境外城市）
        async init() {
            this.$watch('open', val => {
                if (val && this.items.length === 0) this.loadTopLevel();
            });
        },

        async loadTopLevel() {
            this.loading = true;
            this.path = [];
            try {
                const res = await fetch('/tpco/Api/GeoProvinces');
                const data = await res.json();
                this.items = this.tab === 'domestic' ? data.domestic : data.overseas;
            } catch(e) {
                console.error('Geo load failed', e);
            } finally {
                this.loading = false;
            }
        },

        // 下钻到下一级
        async drillDown(node) {
            this.path.push({ id: node.id, title: node.title });
            this.loading = true;
            try {
                const res = await fetch(`/tpco/Api/GeoByProvince?parentId=${node.id}`);
                this.items = await res.json();
                document.getElementById('geoListContainer').scrollTop = 0;
            } catch(e) {
                console.error('Drilldown failed', e);
            } finally {
                this.loading = false;
            }
        },

        // 搜索逻辑
        async search() {
            if (!this.searchQuery) {
                this.loadTopLevel();
                return;
            }
            this.loading = true;
            try {
                const res = await fetch(`/tpco/Api/GeoByProvince?search=${encodeURIComponent(this.searchQuery)}`);
                this.items = await res.json();
            } catch(e) {
                console.error('Search failed', e);
            } finally {
                this.loading = false;
            }
        },

        // 路径导航跳转
        async goToPath(index) {
            const target = this.path[index];
            this.path = this.path.slice(0, index + 1);
            this.loading = true;
            try {
                const res = await fetch(`/tpco/Api/GeoByProvince?parentId=${target.id}`);
                this.items = await res.json();
            } finally {
                this.loading = false;
            }
        },

        resetPath() {
            this.searchQuery = '';
            this.loadTopLevel();
        },

        // 计算属性：按首字母分组
        get groupedItems() {
            const groups = {};
            this.items.forEach(item => {
                // 优先使用后端返回的首字母，如果后端没返回（旧数据），则取标题首位
                const letter = (item.firstLetter || item.title.charAt(0) || '#').toUpperCase();
                
                if (!groups[letter]) groups[letter] = [];
                groups[letter].push({
                    ...item,
                    hasChildren: this.tab === 'domestic' && item.level < 5
                });
            });

            return Object.keys(groups).sort().map(key => ({
                letter: key,
                items: groups[key]
            }));
        },

        hasGroup(letter) {
            return this.groupedItems.some(g => g.letter === letter);
        },

        scrollToGroup(letter) {
            const el = document.getElementById('group-' + letter);
            if (el) {
                el.scrollIntoView({ behavior: 'smooth', block: 'start' });
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
            this.resetPath();
        }
    };
}
