# Contributing Guide — Car Rental System

## Branching Strategy

Використовуємо GitHub Flow:
- `main` — завжди стабільний, захищений branch protection
- Для кожної зміни створюється окрема гілка

### Naming Convention

- `feature/short-description` — нова функціональність
- `fix/short-description` — виправлення помилки
- `refactor/short-description` — рефакторинг
- `docs/short-description` — документація

## Commit Conventions

Використовуємо Conventional Commits:
- `feat:` — нова функціональність
- `fix:` — виправлення помилки
- `refactor:` — рефакторинг без зміни поведінки
- `docs:` — документація
- `test:` — тести
- `chore:` — технічні зміни

Приклади:
- `feat: add CSV export service`
- `fix: restore rental cost after JSON reload`
- `refactor: extract validation to RentalValidator`

## Як створити PR

1. Створи гілку від main за naming convention
2. Зроби зміни і закомть за Conventional Commits
3. Запуш гілку: `git push -u origin feature/your-feature`
4. Відкрий PR на GitHub і заповни шаблон
5. Додай посилання на issue через `Closes #N`
6. Додай рецензента
7. Дочекайся approval і merge

## Як проводити Code Review

Типи коментарів:
- **suggestion** — пропозиція покращення коду
- **question** — запитання щодо рішення
- **nitpick** — дрібне зауваження щодо стилю

Залишай мінімум 3 змістовні коментарі.
Завершуй review з оцінкою: Approve, Request Changes або Comment.

## Як вирішувати конфлікти

```powershell
git checkout feature/your-branch
git merge main
# вирішити конфлікти в редакторі
git add .
git commit -m "fix: resolve merge conflict"
git push
```

Завжди зберігай зміни обох сторін якщо це можливо.