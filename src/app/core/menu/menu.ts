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
      {
        label: 'Assets',
        icon: 'asset',
        route: '/assets',
      },
    ]
  },
  {
    group: 'Admin',
    separator: false,
    items: [
      {
        label: 'Rules',
        icon: 'template',
        route: '/rules',
      },
      {
        icon: 'setting',
        label: 'Settings',
        route: '/settings',
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
