# Информация по пул-реквестам

При отправке пул-реквестов просьба учитывать объем исправлений. Например, добавили языковую форму - пул-реквест. Исправили ошибку запуска - пул-реквест.

Не объединяйте несколько ошибок в одном запросе.


# Технические требования

## Поддерживаемая операционная система

	Windows 7 Service Pack 1; Windows 8; Windows Server 2008 R2 SP1; Windows Server 2012
		Windows 7 с пакетом обновления 1 (SP1) (x86 и x64)
		Windows 8 (x86 и x64)
		Windows Server 2008 R2 с пакетом обновления 1 (SP1) (x64)
		Windows Server 2012 (x64)

## Требования к оборудованию:

	Процессор с тактовой частотой 1,6 ГГц или большей
		ОЗУ объемом 1 ГБ (1,5 ГБ для работы на виртуальной машине)
		10 ГБ доступного пространства на жестком диске
		Жесткий диск с частотой вращения 5400 об/мин
		Видеоадаптер, соответствующий стандарту DirectX 9 и поддерживающий разрешение экрана 1024 x 768 или выше

## Программные требования:

		Visual Studio 2013 или новее
		.NET Framework 4.5.2


# Состав

**!Hell Debug Reader** - программа для чтения информации об ошибках лаунчера.

**!Hell Launcher Cleaner** - очистка лаунчера. Возможно использование для устранения проблем с запуском.

**!Hell Processes to DLL** - упаковка списка процессов в DLL.

**!Hell Process List** - уже забыл для чего, но что-то со списком процессов связано :)

**!Hell Restarter** - перезапуск лаунчера.

**!Hell WPF Multipack Launcher** - сам лаунчер. Используется WPF C#.

**ChangeRevision** - консольная программа для автоматического изменения версии лаунчера при компиляции.

**Processes Library** - вновь что-то с библиотекой процессов.

**!!DLL** - базовые настройки лаунчера, а также готовая библиотека со списком процессов.

**!!other_dll** - список используемых библиотек.

**changelog.json** - лог изменений в ревизиях.


# О нас

Первоначальный разработчик: ["AI RUS - Professional IT support"](http://ai-rus.com), г. Чита

Автор: Andrey Helldar <helldar@ai-rus.com>

Дизайн: [David Jinjolava](https://new.vk.com/jinjolavadavid)

Локализации:

- Английский: [Dmitry Voronov](https://new.vk.com/amiramedia)
- Немецкий: Alexander Storz
- Украинский: [Sergey Metasjov](https://new.vk.com/metasev)
- Русский: [Andrey Helldar](https://new.vk.com/helldar)


# Лицензия

The MIT License (MIT)

Copyright (c) 2014-2016 Andrey Helldar

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.