﻿using System;
using System.Collections.Generic;
using System.IO;
using GodlikeStickCreator.Utilities;
using SevenZip;

namespace GodlikeStickCreator.Core.System.Installer
{
    public class DefaultInstaller : InstallerInfo
    {
        public override InstallMethod InstallMethod { get; } = InstallMethod.Other;

        public override void Install(DirectoryInfo systemDirectory, string systemName, SpecialSnowflake specialSnowflake, string filename, out MenuItemInfo menuItem, SystemProgressReporter progressReporter)
        {
            progressReporter.ReportStatus(InstallationStatus.ExtractFile);

            using (var file = new SevenZipExtractor(filename))
            {
                file.Extracting += (sender, args) => { progressReporter.ReportProgress(args.PercentDone / 100d); };
                file.ExtractArchive(systemDirectory.FullName);
            }

            progressReporter.ReportStatus(InstallationStatus.WriteConfig);
            var configInfo = GetSystemBootInfo(systemDirectory.FullName);
            menuItem = new MenuItemInfo(
                $@"CONFIG /{Path.Combine("multiboot", systemDirectory.Name, configInfo.ConfigPath, configInfo.ConfigFile).Replace('\\', '/')}
APPEND /multiboot/{systemDirectory.Name}/{configInfo.ConfigPath}");

            //For Ubuntu Desktop and derivatives
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux\\txt.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {
                        "initrd=/casper/",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid live-media-path=/multiboot/{systemDirectory.Name}/casper/ initrd=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"kernel /casper/", $"kernel /multiboot/{systemDirectory.Name}/casper/"}
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux\\text.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {
                        "initrd=/casper/",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid live-media-path=/multiboot/{systemDirectory.Name}/casper/ initrd=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"kernel /casper/", $"kernel /multiboot/{systemDirectory.Name}/casper/"}
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux\\isolinux.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {
                        "initrd=/casper/",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid live-media-path=/multiboot/{systemDirectory.Name}/casper/ initrd=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"kernel /casper/", $"kernel /multiboot/{systemDirectory.Name}/casper/"}
                });


            //Alt For derivatives like Dr.Web Livedisk
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "syslinux\\txt.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {
                        "initrd=/casper/",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid live-media-path=/multiboot/{systemDirectory.Name}/casper/ initrd=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"kernel /casper/", $"kernel /multiboot/{systemDirectory.Name}/casper/"}
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "syslinux\\text.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {
                        "initrd=/casper/",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid live-media-path=/multiboot/{systemDirectory.Name}/casper/ initrd=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"kernel /casper/", $"kernel /multiboot/{systemDirectory.Name}/casper/"}
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "syslinux\\syslinux.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {
                        "initrd=/casper/",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid live-media-path=/multiboot/{systemDirectory.Name}/casper/ initrd=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"kernel /casper/", $"kernel /multiboot/{systemDirectory.Name}/casper/"}
                });

            //Rename the following for grub loopback.cfg
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "boot\\grub\\loopback.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {"/casper/vmlinuz", $"/multiboot/{systemDirectory.Name}/casper/vmlinuz"},
                    {"/casper/initrd", $"/multiboot/{systemDirectory.Name}/casper/initrd"},
                    {
                        "boot=casper",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid boot=NULL live-media-path=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"boot=NULL", "boot=casper"},
                });

            //Rename the following for grub.cfg
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "boot\\grub\\grub.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {"/casper/vmlinuz", $"/multiboot/{systemDirectory.Name}/casper/vmlinuz"},
                    {"/casper/initrd", $"/multiboot/{systemDirectory.Name}/casper/initrd"},
                    {
                        "boot=casper",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid boot=NULL live-media-path=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"boot=NULL", "boot=casper"},

                });


            //For Debian Based and derivatives
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux", "live.cfg"),
                new Dictionary<string, string>
                {
                    {"linux /live/", $"linux /multiboot/{systemDirectory.Name}/live/"},
                    {"initrd /live/", $"initrd /multiboot/{systemDirectory.Name}/live/"},
                    {
                        "append boot=live",
                        $"append live-media-path=/multiboot/{systemDirectory.Name}/live/ cdrom-detect/try-usb=true noprompt boot=live"
                    }
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux", "install.cfg"),
                new Dictionary<string, string>
                {
                    {"linux /install/", $"linux /multiboot/{systemDirectory.Name}/install/"},
                    {"initrd /install/", $"initrd /multiboot/{systemDirectory.Name}/install/"},
                    {"-- quiet", "cdrom-detect/try-usb=true quiet"}
                });

            //SolydX
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, configInfo.ConfigPath, configInfo.ConfigFile),
                new Dictionary<string, string>
                {
                    {"kernel /solydxk", $"kernel /multiboot/{systemDirectory.Name}/solydxk"},
                    {"initrd=/solydxk", $"initrd=/multiboot/{systemDirectory.Name}/solydxk"},
                    {"live-media-path=/solydxk", $"live-media-path=/multiboot/{systemDirectory.Name}/solydxk"},
                });

            //For Desinfect Distro
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux\\os.cfg"),
                new Dictionary<string, string>
                {
                    {"file=/cdrom/preseed/", $"file=/cdrom/multiboot/{systemDirectory.Name}/preseed/"},
                    {
                        "initrd=/casper/",
                        $"cdrom-detect/try-usb=true noprompt floppy.allowed_drive_mask=0 ignore_uuid live-media-path=/multiboot/{systemDirectory.Name}/casper/ initrd=/multiboot/{systemDirectory.Name}/casper/"
                    },
                    {"kernel /casper/", $"kernel /multiboot/{systemDirectory.Name}/casper/"},
                    {"BOOT_IMAGE=/casper/", $"BOOT_IMAGE=/multiboot/{systemDirectory.Name}/casper/"},
                });

            //For Fedora Based and derivatives
            if (File.Exists(Path.Combine(systemDirectory.FullName, "isolinux\\isolinux.cfg")) &&
                File.Exists(Path.Combine(systemDirectory.FullName, "LiveOS\\livecd-iso-to-disk"))) //Probably Fedora based
            {
                File.WriteAllText(Path.Combine(systemDirectory.FullName, "isolinux\\isolinux.cfg"),
                    File.ReadAllText(
                        Path.Combine(systemDirectory.FullName, "isolinux\\isolinux.cfg")
                            .Replace("root=live:CDLABEL=",
                                $"root=live:LABEL=MULTIBOOT live_dir=/multiboot/{systemDirectory.Name}/LiveOS NULL=")));
            }

            //For Puppy Based and derivatives
            if (File.Exists(Path.Combine(systemDirectory.FullName, "isolinux.cfg")))
            {
                if (File.Exists(Path.Combine(systemDirectory.FullName, "help2.msg"))) //Probably Puppy based 
                    ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux.cfg"),
                        new Dictionary<string, string>
                        {
                            {"pmedia=cd", $"psubdir=/multiboot/{systemDirectory.Name} psubok=TRUE"},
                        });
                else if (File.Exists(Path.Combine(systemDirectory.FullName, "help.msg"))) //Probably Puppy based
                    ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux.cfg"),
                        new Dictionary<string, string>
                        {
                            {"append search", $"append search psubdir=/multiboot/{systemDirectory.Name} psubok=TRUE"},
                        });
            }

            //For Clonezilla, and DRBL
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "syslinux\\syslinux.cfg"),
                new Dictionary<string, string>
                {
                    {"kernel /live", $"kernel /multiboot/{systemDirectory.Name}/live"},
                    {
                        "initrd=/live",
                        $"live-media-path=/multiboot/{systemDirectory.Name}/live/ initrd=/multiboot/{systemDirectory.Name}/live"
                    }
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux\\isolinux.cfg"),
                new Dictionary<string, string>
                {
                    {"kernel /live", $"kernel /multiboot/{systemDirectory.Name}/live"},
                    {
                        "initrd=/live",
                        $"live-media-path=/multiboot/{systemDirectory.Name}/live/ initrd=/multiboot/{systemDirectory.Name}/live"
                    },
                    {"live-media-path=/live", $"live-media-path=/multiboot/{systemDirectory.Name}/live"} //Linux Mint 2014
                });

            //Xpud
            if (File.Exists(Path.Combine(systemDirectory.FullName, "boot\\vesamenu.c32")) &&
                File.Exists(Path.Combine(systemDirectory.FullName, "opt\\media")))
                ReplaceStringInFile(Path.Combine(systemDirectory.FullName, configInfo.ConfigPath, configInfo.ConfigFile),
                    new Dictionary<string, string>
                    {
                        {"KERNEL /boot/", $"KERNEL /multiboot/{systemDirectory.Name}/boot/"},
                        {
                            "/opt/media,/opt/scim",
                            $"/multiboot/{systemDirectory.Name}/opt/media,/multiboot/{systemDirectory.Name}/opt/scim"
                        },
                        {"DEFAULT /boot/", $"DEFAULT /multiboot/{systemDirectory.Name}/boot/"},
                        {"MENU BACKGROUND /boot/", $"MENU BACKGROUND /multiboot/{systemDirectory.Name}/boot/"},
                        {"APPEND initrd=/opt/media", $"APPEND initrd=/multiboot/{systemDirectory.Name}/opt/media"},
                    });

            //Panda Safe CD, Tails
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, Path.Combine(configInfo.ConfigPath.Replace('/', '\\'), "live.cfg")),
                new Dictionary<string, string>
                {
                    {"kernel /live", $"kernel /multiboot/{systemDirectory.Name}/live"},
                    {"initrd=/live", $"initrd=/multiboot/{systemDirectory.Name}/live"},
                    {
                        "append initrd=", $"append noprompt live-media-path=/multiboot/{systemDirectory.Name}/live initrd="
                    }
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, Path.Combine(configInfo.ConfigPath.Replace('/', '\\'), "live486.cfg")), //Tails Specific 486
                new Dictionary<string, string>
                {
                    {"kernel /live", $"kernel /multiboot/{systemDirectory.Name}/live"},
                    {"initrd=/live", $"initrd=/multiboot/{systemDirectory.Name}/live"},
                    {
                        "append initrd=", $"append noprompt live-media-path=/multiboot/{systemDirectory.Name}/live initrd="
                    }
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, Path.Combine(configInfo.ConfigPath.Replace('/', '\\'), "live686.cfg")), //Tails Specific
                new Dictionary<string, string>
                {
                    {"kernel /live", $"kernel /multiboot/{systemDirectory.Name}/live"},
                    {"initrd=/live", $"initrd=/multiboot/{systemDirectory.Name}/live"},
                    {
                        "append initrd=", $"append noprompt live-media-path=/multiboot/{systemDirectory.Name}/live initrd="
                    }
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, Path.Combine(configInfo.ConfigPath.Replace('/', '\\'), configInfo.ConfigFile)),
                new Dictionary<string, string>
                {
                    {
                        "default /isolinux/vesamenu.c32",
                        $"default /multiboot/{systemDirectory.Name}/isolinux/vesamenu.c32"
                    }
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, Path.Combine(configInfo.ConfigPath.Replace('/', '\\'), "stdmenu.cfg")),
                new Dictionary<string, string>
                {
                    {"menu background /isolinux", $"menu background /multiboot/{systemDirectory.Name}/isolinux"},
                });
            ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux\\liveamd64.cfg"), //Tails Specific
                new Dictionary<string, string>
                {
                    {"kernel /live", $"kernel /multiboot/{systemDirectory.Name}/live"},
                    {"initrd=/live", $"initrd=/multiboot/{systemDirectory.Name}/live"},
                    {
                        "append initrd=", $"append noprompt live-media-path=/multiboot/{systemDirectory.Name}/live initrd="
                    }
                });

            switch (specialSnowflake)
            {
                case SpecialSnowflake.SystemRescueDisk:
                    ReplaceStringInFile(Path.Combine(systemDirectory.FullName, configInfo.ConfigPath, configInfo.ConfigFile),
                        new Dictionary<string, string>
                        {
                            {
                                "INITRD initram.igz",
                                $"INITRD NULL initram.igz\r\nAPPEND subdir=multiboot/{systemDirectory.Name}"
                            },
                            {"INITRD NULL initram.igz", "INITRD initram.igz"},
                            {"APPEND docache", $"APPEND subdir=multiboot/{systemDirectory.Name} docache"},
                            {"APPEND nomodeset", $"APPEND subdir=multiboot/{systemDirectory.Name} nomodeset"},
                            {"APPEND video=800x600", $"APPEND subdir=multiboot/{systemDirectory.Name} video=800x600"},
                            {"APPEND root=auto", $"APPEND subdir=multiboot/{systemDirectory.Name} root=auto"},
                            {"APPEND dostartx", $"APPEND subdir=multiboot/{systemDirectory.Name} dostartx"},
                            {"kernel /bootdisk", $"kernel /multiboot/{systemDirectory.Name}/bootdisk"},
                            {"kernel /ntpasswd", $"kernel /multiboot/{systemDirectory.Name}/ntpasswd"},
                            {
                                "/ntpasswd/initrd.cgz,/ntpasswd/scsi.cgz",
                                $"/multiboot/{systemDirectory.Name}/ntpasswd/initrd.cgz,/multiboot/{systemDirectory.Name}/ntpasswd/scsi.cgz"
                            },
                            {"initrd=/bootdisk", $"initrd=/multiboot/{systemDirectory.Name}/bootdisk"},
                        });
                    break;
                case SpecialSnowflake.IsoLinuxPrompt0:
                    var isoLinuxFile = new FileInfo(Path.Combine(systemDirectory.FullName, "isolinux.cfg"));
                    if (!isoLinuxFile.Exists)
                        break; //not important enough to crash, it's just a comfort solution to prevent that boot shit

                    var content = File.ReadAllText(isoLinuxFile.FullName);
                    File.WriteAllText(isoLinuxFile.FullName, content.Replace("PROMPT 1", "PROMPT 0", StringComparison.OrdinalIgnoreCase));
                    break;
                case SpecialSnowflake.UbuntuStudio:
                    ReplaceStringInFile(Path.Combine(systemDirectory.FullName, "isolinux", "isolinux.cfg"),
                        new Dictionary<string, string>
                        {
                            {"ui gfxboot bootlogo", "#ui gfxboot bootlogo"}
                        });
                    break;
            }
        }
    }
}