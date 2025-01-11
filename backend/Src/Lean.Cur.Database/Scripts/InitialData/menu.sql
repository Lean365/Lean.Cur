-- 菜单初始化脚本
-- 作者：CodeGenerator
-- 日期：2024-01-08
-- 版本：1.0.0
-- 说明：初始化系统基础菜单数据

-- 清空已有数据
DELETE FROM lean_role_menu;
DELETE FROM lean_menu;

-- 重置自增ID
DBCC CHECKIDENT ('lean_menu', RESEED, 0);

-- 系统管理
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('系统管理', 'system', NULL, 1, '/system', NULL, 0, 0, 'M', 1, 1, NULL, 'setting', '系统管理目录', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 用户管理
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('用户管理', 'user', 1, 1, '/system/user', '/system/user/index', 0, 0, 'C', 1, 1, 'system:user:list', 'user', '用户管理菜单', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 角色管理
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('角色管理', 'role', 1, 2, '/system/role', '/system/role/index', 0, 0, 'C', 1, 1, 'system:role:list', 'team', '角色管理菜单', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 菜单管理
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('菜单管理', 'menu', 1, 3, '/system/menu', '/system/menu/index', 0, 0, 'C', 1, 1, 'system:menu:list', 'menu', '菜单管理菜单', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 部门管理
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('部门管理', 'dept', 1, 4, '/system/dept', '/system/dept/index', 0, 0, 'C', 1, 1, 'system:dept:list', 'apartment', '部门管理菜单', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 岗位管理
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('岗位管理', 'post', 1, 5, '/system/post', '/system/post/index', 0, 0, 'C', 1, 1, 'system:post:list', 'solution', '岗位管理菜单', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 用户管理按钮
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('用户查询', 'user:query', 2, 1, '', '', 0, 0, 'F', 1, 1, 'system:user:query', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('用户创建', 'user:create', 2, 2, '', '', 0, 0, 'F', 1, 1, 'system:user:create', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('用户修改', 'user:edit', 2, 3, '', '', 0, 0, 'F', 1, 1, 'system:user:edit', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('用户删除', 'user:delete', 2, 4, '', '', 0, 0, 'F', 1, 1, 'system:user:delete', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('用户导出', 'user:export', 2, 5, '', '', 0, 0, 'F', 1, 1, 'system:user:export', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('用户导入', 'user:import', 2, 6, '', '', 0, 0, 'F', 1, 1, 'system:user:import', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('重置密码', 'user:resetPwd', 2, 7, '', '', 0, 0, 'F', 1, 1, 'system:user:resetPwd', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 角色管理按钮
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('角色查询', 'role:query', 3, 1, '', '', 0, 0, 'F', 1, 1, 'system:role:query', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('角色创建', 'role:create', 3, 2, '', '', 0, 0, 'F', 1, 1, 'system:role:create', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('角色修改', 'role:edit', 3, 3, '', '', 0, 0, 'F', 1, 1, 'system:role:edit', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('角色删除', 'role:delete', 3, 4, '', '', 0, 0, 'F', 1, 1, 'system:role:delete', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('角色导出', 'role:export', 3, 5, '', '', 0, 0, 'F', 1, 1, 'system:role:export', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 菜单管理按钮
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('菜单查询', 'menu:query', 4, 1, '', '', 0, 0, 'F', 1, 1, 'system:menu:query', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('菜单创建', 'menu:create', 4, 2, '', '', 0, 0, 'F', 1, 1, 'system:menu:create', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('菜单修改', 'menu:edit', 4, 3, '', '', 0, 0, 'F', 1, 1, 'system:menu:edit', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('菜单删除', 'menu:delete', 4, 4, '', '', 0, 0, 'F', 1, 1, 'system:menu:delete', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 部门管理按钮
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('部门查询', 'dept:query', 5, 1, '', '', 0, 0, 'F', 1, 1, 'system:dept:query', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('部门创建', 'dept:create', 5, 2, '', '', 0, 0, 'F', 1, 1, 'system:dept:create', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('部门修改', 'dept:edit', 5, 3, '', '', 0, 0, 'F', 1, 1, 'system:dept:edit', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('部门删除', 'dept:delete', 5, 4, '', '', 0, 0, 'F', 1, 1, 'system:dept:delete', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 岗位管理按钮
INSERT INTO lean_menu (menu_name, menu_code, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, remark, create_by, create_time, update_by, update_time, deleted)
VALUES 
('岗位查询', 'post:query', 6, 1, '', '', 0, 0, 'F', 1, 1, 'system:post:query', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('岗位创建', 'post:create', 6, 2, '', '', 0, 0, 'F', 1, 1, 'system:post:create', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('岗位修改', 'post:edit', 6, 3, '', '', 0, 0, 'F', 1, 1, 'system:post:edit', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('岗位删除', 'post:delete', 6, 4, '', '', 0, 0, 'F', 1, 1, 'system:post:delete', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0),
('岗位导出', 'post:export', 6, 5, '', '', 0, 0, 'F', 1, 1, 'system:post:export', '#', '', 'admin', GETDATE(), 'admin', GETDATE(), 0);

-- 为超级管理员角色分配所有菜单权限
INSERT INTO lean_role_menu (role_id, menu_id, create_by, create_time, update_by, update_time, deleted)
SELECT 1, id, 'admin', GETDATE(), 'admin', GETDATE(), 0
FROM lean_menu; 