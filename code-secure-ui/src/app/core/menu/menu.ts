import {MenuItem} from "./menu.model";

export const items: MenuItem[] = [
  {
    group: 'Application',
    separator: false,
    items: [
      {
        icon: 'chart-pie',
        label: 'Dashboard',
        route: '/dashboard',
      },
      {
        icon: 'asset',
        label: 'Projects',
        route: '/project',
      },
    ]
  },
  {
    group: 'Admin',
    separator: false,
    items: [
      {
        label: 'User Manager',
        icon: 'users',
        route: '/user',
      },
      {
        icon: 'setting',
        label: 'Settings',
        route: '/setting',
      },
      // {
      //   icon: 'assets/icons/heroicons/outline/folder.svg',
      //   label: 'Folders',
      //   route: '/folders',
      //   children: [
      //     { label: 'Current Files', route: '/folders/current-files' },
      //     { label: 'Downloads', route: '/folders/download' },
      //     { label: 'Trash', route: '/folders/trash' },
      //   ],
      // },
    ],
  },
];
