function geoPicker(fieldName, initId, initName) {
    return {
        open: false,
        tab: 'domestic',
        loading: false,
        searchQuery: '',
        
        // 最终确认选中的 ID 和名称（用于与父页面表单绑定）
        selectedId: initId || null,
        selectedName: initName || '',

        // 临时选中的节点（仅在弹窗内高亮和显示，点击底部“确认”才真正应用）
        tempSelectedId: initId || null,
        tempSelectedName: initName || '',

        // 国内级联状态池
        provinces: [],
        cities: [],
        counties: [],
        towns: [],
        villages: [],

        // 境外级联状态池
        countries: [],
        overseasTowns: [],

        // 当前激活的各级节点对象
        activeProvince: null,
        activeCity: null,
        activeCounty: null,
        activeTown: null,
        activeVillage: null,
        
        activeCountry: null,
        activeOverseasTown: null,

        // 仅在搜索模式下使用的平铺数据列表（支持 A-Z 索引）
        searchResults: [],

        // 初始化加载顶级
        async init() {
            this.$watch('open', val => {
                if (val) {
                    this.syncTempFromSelected();
                    if (this.provinces.length === 0 && this.countries.length === 0) {
                        this.loadTopLevel();
                    }
                }
            });
            this.$watch('tab', () => {
                this.clearCascadeState();
                this.loadTopLevel();
            });
        },

        // 将当前选中的状态同步为临时选中
        syncTempFromSelected() {
            this.tempSelectedId = this.selectedId;
            this.tempSelectedName = this.selectedName;
        },

        // 清空级联面板的所有临时状态
        clearCascadeState() {
            this.activeProvince = null;
            this.activeCity = null;
            this.activeCounty = null;
            this.activeTown = null;
            this.activeVillage = null;
            this.activeCountry = null;
            this.activeOverseasTown = null;

            this.cities = [];
            this.counties = [];
            this.towns = [];
            this.villages = [];
            this.overseasTowns = [];
        },

        // 加载顶级列表（省份或国家）
        async loadTopLevel() {
            this.loading = true;
            try {
                const res = await fetch('/tpco/Api/GeoProvinces');
                const data = await res.json();
                if (this.tab === 'domestic') {
                    this.provinces = data.Domestic || data.domestic || [];
                } else {
                    this.countries = data.Overseas || data.overseas || [];
                }
            } catch(e) {
                console.error('加载顶级地区失败', e);
            } finally {
                this.loading = false;
            }
        },

        // 面板点击联动核心逻辑
        async selectNode(item, rowIndex) {
            this.loading = true;
            try {
                if (this.tab === 'domestic') {
                    if (rowIndex === 1) {
                        this.activeProvince = item;
                        this.activeCity = this.activeCounty = this.activeTown = this.activeVillage = null;
                        this.cities = []; this.counties = []; this.towns = []; this.villages = [];
                    } else if (rowIndex === 2) {
                        this.activeCity = item;
                        this.activeCounty = this.activeTown = this.activeVillage = null;
                        this.counties = []; this.towns = []; this.villages = [];
                    } else if (rowIndex === 3) {
                        this.activeCounty = item;
                        this.activeTown = this.activeVillage = null;
                        this.towns = []; this.villages = [];
                    } else if (rowIndex === 4) {
                        this.activeTown = item;
                        this.activeVillage = null;
                        this.villages = [];
                    } else if (rowIndex === 5) {
                        this.activeVillage = item;
                    }
                } else {
                    if (rowIndex === 1) {
                        this.activeCountry = item;
                        this.activeOverseasTown = null;
                        this.overseasTowns = [];
                    } else if (rowIndex === 2) {
                        this.activeOverseasTown = item;
                    }
                }

                // 暂存选中的 ID
                this.tempSelectedId = item.id;
                this.tempSelectedName = this.buildPathName();

                // 如果不是最末级，且不是直接选定的村庄，需要去加载下级并进行“Level 分流归类”
                if ((this.tab === 'domestic' && rowIndex < 5) || (this.tab === 'overseas' && rowIndex === 1)) {
                    const res = await fetch(`/tpco/Api/GeoByProvince?parentId=${item.id}`);
                    const children = await res.json();
                    
                    if (this.tab === 'domestic') {
                        children.forEach(child => {
                            if (child.level === 3) this.cities.push(child);
                            else if (child.level === 4) this.counties.push(child);
                            else if (child.level === 5) this.towns.push(child);
                            else if (child.level === 6) this.villages.push(child);
                        });
                    } else {
                        this.overseasTowns = children;
                    }
                }
            } catch(e) {
                console.error('联动加载失败', e);
            } finally {
                this.loading = false;
            }
        },

        // 构建当前选择的面包屑完整路径名，用于底部展示
        buildPathName() {
            const parts = [];
            if (this.tab === 'domestic') {
                if (this.activeProvince) parts.push(this.activeProvince.title);
                if (this.activeCity) parts.push(this.activeCity.title);
                if (this.activeCounty) parts.push(this.activeCounty.title);
                if (this.activeTown) parts.push(this.activeTown.title);
                if (this.activeVillage) parts.push(this.activeVillage.title);
            } else {
                if (this.activeCountry) parts.push(this.activeCountry.title);
                if (this.activeOverseasTown) parts.push(this.activeOverseasTown.title);
            }
            return parts.join(' > ');
        },

        // 节点是否高亮激活
        isActive(item, rowIndex) {
            if (this.tab === 'domestic') {
                if (rowIndex === 1) return this.activeProvince?.id === item.id;
                if (rowIndex === 2) return this.activeCity?.id === item.id;
                if (rowIndex === 3) return this.activeCounty?.id === item.id;
                if (rowIndex === 4) return this.activeTown?.id === item.id;
                if (rowIndex === 5) return this.activeVillage?.id === item.id;
            } else {
                if (rowIndex === 1) return this.activeCountry?.id === item.id;
                if (rowIndex === 2) return this.activeOverseasTown?.id === item.id;
            }
            return false;
        },

        // 搜索逻辑
        async search() {
            if (!this.searchQuery) {
                this.searchResults = [];
                return;
            }
            this.loading = true;
            try {
                const res = await fetch(`/tpco/Api/GeoByProvince?search=${encodeURIComponent(this.searchQuery)}`);
                this.searchResults = await res.json();
            } catch(e) {
                console.error('搜索地区失败', e);
            } finally {
                this.loading = false;
            }
        },

        // 在扁平搜索列表里直接选中
        selectSearchItem(item) {
            this.tempSelectedId = item.id;
            this.tempSelectedName = item.parentTitle ? `${item.parentTitle} > ${item.title}` : item.title;
        },

        // 计算属性：将搜索结果按拼音首字母分组（仅在搜索模式下展示）
        get groupedItems() {
            const groups = {};
            if (!this.searchResults) return [];

            this.searchResults.forEach(item => {
                let fl = item.firstLetter || item.FirstLetter;
                let letter = (fl || item.title.charAt(0) || '#').toUpperCase();
                
                if (!groups[letter]) groups[letter] = [];
                groups[letter].push(item);
            });

            return Object.keys(groups).sort().map(key => ({
                letter: key,
                items: groups[key]
            }));
        },

        // 滚动到指定字母分组
        scrollToGroup(letter) {
            const container = document.getElementById('geoListContainer');
            const target = document.getElementById('group-' + letter);
            if (container && target) {
                const top = target.offsetTop - container.offsetTop;
                container.scrollTo({
                    top: top,
                    behavior: 'smooth'
                });
            }
        },

        // 底部“确认”提交
        confirmSelection() {
            if (this.tempSelectedId) {
                this.selectedId = this.tempSelectedId;
                // 回填名称到按钮上时，只保留最后一级叶子节点的名称
                const parts = this.tempSelectedName.split(' > ');
                this.selectedName = parts[parts.length - 1];
            } else {
                this.selectedId = null;
                this.selectedName = '';
            }
            this.open = false;
        },

        // 清除选择
        clear() {
            this.selectedId = null;
            this.selectedName = '';
            this.tempSelectedId = null;
            this.tempSelectedName = '';
            this.clearCascadeState();
            this.searchResults = [];
            this.searchQuery = '';
            this.loadTopLevel();
        }
    };
}
