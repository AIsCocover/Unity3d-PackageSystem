PackageSystem
背包系统
=================================
主要的UI结构：
	PackageUI(Canvas)
		Inventory					// 背包主面板
			ItemSlots				// 物品槽根结点
				item_slot_x			// 第x个槽（x从0开始）
					raycast_image	// 用于接收射线检测
					slot_images
						background	// 物品槽背景图
						icon		// 物品图标
					close_button	// 丢弃按钮
			title					// 背包标题
			tooltipPanel			// 用于显示物品的详细信息，随鼠标移动到物品上显示，移开时隐藏
				background			// 背景图
				content				// 内容
		AddButton					// 随机添加一个物品
		SortButton					// 整理背包，简单整理：仅将物品尽可能地前移
		ClearButton					// 清空背包
项目结构（脚本部分）
	主要将整体业务分为UI层和Logic层
		Logic层处理底层逻辑业务，封装方法；
		UI层处理UI相关业务，通过调用Logic层封装好的方法实现功能。
	Scripts
		Inventory				// 存放Inventory相关代码
			UI
				InventoryUI.cs	// 管理于Inventory UI有关的代码（监听鼠标事件等）
				ItemSlotUI.cs	// 管理与ItemSlot UI有关的代码（鼠标事件等）
			Inventory.cs		// Items管理器，单例
			ItemSlot.cs			// 物品槽实体，属于UI层实体
		Items					// 存放Items相关代码
			Item.cs				// 物品实体，ScriptableObject，基类，属于Logic层实体
			Equipment.cs		// 装备实体，ScriptableObject，继承自Item，属于Logic层实体
			Potion.cs			// 药剂实体，ScriptableObject，继承自Item，属于Logic层实体
		ToolTip					// 存放ToolTipPanel相关代码
			UI
				TooltipUI.cs	// ToolTipPanel UI有关的代码（更新内容、显示、隐藏等）
		Toolkits				// 存放Toolkits相关代码，用于提供额外的功能帮助，例如自动生成一定数量的item_slot，C#深拷贝的实现。
			InventoryTool.cs	
	
	Logic层主要维护一个items列表，items列表用于存放当前背包中的物品以及物品的所在位置。其中列表索引表示物品所在的物品槽ID，对应的值为物品本身。
		例如：items[0]=[Equipment] 表示在ID为0的物品槽下装有一个Equipment类型的物品。
	UI层处理所有会改变UI的代码（通过Logic层提供的方法实现功能），比如图片拖拽、鼠标点击的响应等。
	
	
	