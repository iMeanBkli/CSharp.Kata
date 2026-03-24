# Longest Palindrome

---

## Statement

You are given a string that may contain **uppercase and lowercase letters**, **accented characters**, and **spaces**.

Your task is to find and return the **longest palindrome** that can be found within that string. If multiple palindromes share the same maximum length, return the **first one found** (i.e. the one whose starting index is encountered first during detection).

---

## Normalization Rules

Before evaluating whether a substring is a palindrome, apply the following normalization — **for comparison only**. The returned result must always preserve the **original characters** (original casing and accents).

**Case** is ignored:
- `"Ete"` and `"ete"` are treated as equivalent.

**Spaces** are ignored during comparison:
- `"never odd or even"` is a valid palindrome — spaces are stripped before comparison.

**Accented characters** are mapped to their base letter:

| Accented    | Base letter |
|:-----------:|:-----------:|
| `é è ê ë`  | `e`         |
| `à â ä`    | `a`         |
| `î ï`      | `i`         |
| `ô ö`      | `o`         |
| `ù û ü`    | `u`         |
| `ç`        | `c`         |

> **Example:** `"Éte"` normalizes to `"ete"`, which reversed is `"ete"` — it is a palindrome.  
> The returned value is `"Éte"` (original form).

---

## Return Value

- Return the **original substring** (casing and accents preserved) corresponding to the longest palindrome found.
- A **single character** is a valid palindrome (e.g. `"a"`).
- If the input is empty or no palindrome can be determined, return `""`.

> [!IMPORTANT]
> The returned string must be the **original** substring extracted from the input — **not** its normalized form.

> [!WARNING]
> Spaces are stripped **for comparison only**. The returned substring must include the original spaces.  
> For example, for input `"never odd or even"`, return `"never odd or even"`, not `"neveroddoreven"`.

---

## Examples

### Example 1 — Basic palindrome

```plaintext
Input   : "racecar"
Output  : "racecar"
```

`"racecar"` normalized → `"racecar"` → reversed `"racecar"` ✓

---

### Example 2 — Case insensitivity

```plaintext
Input   : "Ete"
Output  : "Ete"
```

`"Ete"` normalized → `"ete"` → reversed `"ete"` ✓  
Returned as original: `"Ete"`.

---

### Example 3 — Accented characters

```plaintext
Input   : "Éte"
Output  : "Éte"
```

`"Éte"` normalized → `"ete"` → reversed `"ete"` ✓  
Returned as original: `"Éte"`.

---

### Example 4 — Sentence palindrome (spaces ignored)

```plaintext
Input   : "never odd or even"
Output  : "never odd or even"
```

Stripped + lowercased → `"neveroddoreven"` → reversed `"neveroddoreven"` ✓  
Returned as original: `"never odd or even"`.

---

### Example 5 — Multiple palindromes, return the longest

```plaintext
Input   : "abacaba"
Output  : "abacaba"
```

Candidates: `"aba"` (index 0), `"acaca"` (index 1), `"abacaba"` (index 0) — longest wins.

---

### Example 6 — Tie on length, return the first found

```plaintext
Input   : "abbacddc"
Output  : "abba"
```

`"abba"` (length 4, index 0) and `"cddc"` (length 4, index 4) are both valid.  
`"abba"` is returned as it is encountered first.

---

### Example 7 — No palindrome of length ≥ 2

```plaintext
Input   : ""
Output  : ""
```

---

## Edge Cases

> [!WARNING]
> A substring made of a **single character** is always a valid palindrome — but only returned if no longer palindrome exists.

> [!WARNING]
> Do **not** strip spaces from the returned string — preserve the original substring exactly as it appears in the input.

> [!TIP]
> Normalize the entire input once before searching (lowercase + accent replacement + ignore spaces for comparison), but always track positions against the **original** string to reconstruct the result.

---

## Solution Levels

### Level 1 - Easy

- [ ] Comparison is **case-insensitive**
- [ ] Spaces are **ignored for comparison**, preserved in output
~~- [ ] Accented characters are **mapped to their base letter** before comparison~~
- [ ] A **single character** is a valid palindrome
- [ ] On length tie, return the **first encountered** palindrome
- [ ] Return `""` if input is empty
- [ ] Handle strings containing **only spaces**
- [ ] Handle strings where **every character** is the same (e.g. `"aaaa"`)

### Level 2 - Medium

- [ ] Comparison is **case-insensitive**
- [ ] Spaces are **ignored for comparison**, preserved in output
- [ ] Accented characters are **mapped to their base letter** before comparison
- [ ] A **single character** is a valid palindrome
- [ ] On length tie, return the **first encountered** palindrome
- [ ] Return `""` if input is empty
- [ ] Handle strings containing **only spaces**
- [ ] Handle strings where **every character** is the same (e.g. `"aaaa"`)
- [ ] Solution complexity is better than O(n²)

### Level 3 - Hard

- [ ] Comparison is **case-insensitive**
- [ ] Spaces are **ignored for comparison**, preserved in output
- [ ] Accented characters are **mapped to their base letter** before comparison
- [ ] A **single character** is a valid palindrome
- [ ] On length tie, return the **first encountered** palindrome
- [ ] Return `""` if input is empty
- [ ] Handle strings containing **only spaces**
- [ ] Handle strings where **every character** is the same (e.g. `"aaaa"`)
- [ ] Solution should have a O(1) memory consumption.

