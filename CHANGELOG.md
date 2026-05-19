# Changelog

## [1.0.0] — 2026-05-20

### Added
- Фінальна документація: USER_GUIDE.md, DEVELOPER_GUIDE.md, FINAL_REPORT.md
- DEMO.md з покроковим сценарієм захисту
- docs/defense-qa.md з питаннями і відповідями
- docs/syllabus-coverage.md — матриця покриття курсу
- docs/release-plan.md
- XML-коментарі до публічних API

### Changed
- README.md оновлено з посиланнями на всю документацію
- Фінальний рефакторинг — усунення smells

## [0.3.0] — 2026-05-18

### Added
- 50 тестів (42 юніт + 8 інтеграційних)
- Coverage з coverlet
- TESTING.md, docs/test-strategy.md, docs/test-matrix.md
- docs/iteration-3.md

### Fixed
- Некоректне відновлення доступності авто після reload JSON
- Дублювання тестового коду винесено в TestHelpers

## [0.2.0] — 2026-04-29

### Added
- JSON persistence (JsonCarRepository, JsonClientRepository, JsonRentalRepository)
- Strategy патерн: StandardPricingStrategy, DiscountPricingStrategy
- Command патерн: ICommand, AppRunner, 8 команд меню
- LINQ-запити: фільтрація, сортування, агрегація
- Скасування оренди, аналітика
- docs/iteration-2.md, docs/iteration-2-plan.md

### Changed
- Program.cs рефакторинг з 200 до 46 рядків
- Switch замінено на Dictionary команд

## [0.1.0] — 2026-04-29

### Added
- Доменна модель: Car, Client, Rental, RentalStatus
- Інтерфейси репозиторіїв: ICarRepository, IClientRepository, IRentalRepository
- In-memory репозиторії
- RentalService з базовою бізнес-логікою
- Консольне меню — перший вертикальний зріз
- GitHub Actions CI
- docs/vision.md, docs/backlog.md, діаграми, docs/iteration-1.md