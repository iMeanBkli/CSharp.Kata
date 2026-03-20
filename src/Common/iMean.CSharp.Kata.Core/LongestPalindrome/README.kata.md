# Kata Name (Example)

---

## Statement

Each lowercase letter has a value equal to its position in the alphabet:
`a = 1`, `b = 2`, `c = 3` ... `z = 26`. **Spaces are ignored.**

Given a list of strings, compute for each string its **letter sum**, 
then multiply it by the **1-based position** of that string in the list. 
Return the list of results.

---

## Input

```plaintext
Line 1      : int N        — number of strings (1 ≤ N ≤ 1000)
Lines 2…N+1 : string       — one string per line, lowercase letters and spaces only
                             (1 ≤ length ≤ 1000 characters, at least one non-space letter)
```

---

## Output

```plaintext
N integers separated by spaces on a single line
```

---

## Constraints

| Variable         | Min  | Max        | Notes                        |
|:-----------------|:----:|:----------:|:-----------------------------|
| `N`              | `1`  | `1 000`    | Number of strings            |
| String length    | `1`  | `1 000`    | Chars including spaces       |
| Letter value     | `1`  | `26`       | `a = 1` … `z = 26`           |
| Max output value | —    | `26 000 000` | Fits safely in `int`       |

- Time limit : **1 second**
- Memory limit : **256 MB**
- Input contains **only** lowercase characters (`a`–`z`) and spaces

---

## Example

```plaintext
Input           Output
2               6 24
abc
abc abc
```

### Explanation

| Position | String      | Letter sum                                      | × position  | Result |
|:--------:|:------------|:-----------------------------------------------:|:-----------:|:------:|
| `1`      | `"abc"`     | `1 + 2 + 3 = 6`                                 | `× 1`       | `6`    |
| `2`      | `"abc abc"` | `1 + 2 + 3 + 1 + 2 + 3 = 12` *(spaces ignored)* | `× 2`       | `24`   |

> **Note :** the space between the two `abc` in `"abc abc"` is ignored — only letter values are summed.

---

## Edge Cases

> [!WARNING]
> A string may contain **leading, trailing, or multiple consecutive spaces** — all must be ignored.

> [!IMPORTANT]
> Position starts at **1**, not 0.

> [!TIP]
> The maximum possible output value per element is `26 × 1000 × 1000 = 26 000 000`, 
which fits in a standard `int` (max ~2.1 billion).

---

## Checklist

- [ ] Item 1
- [ ] Item 2
- [ ] Item 3
- [ ] Item 4
