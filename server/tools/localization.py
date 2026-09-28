"""Reviewed Traditional Chinese spellings shared by conversion and auditing."""
from opencc import OpenCC

def traditional_text(converter: OpenCC, text: str) -> str:
    result = converter.convert(text)
    # These nouns are already valid Traditional Chinese; the alternative forms
    # are also valid, so do not require a spelling change when auditing them.
    for word in ('\u80cc\u5305', '\u5bb6\u5177', '\u7fa4',
                 '\u90c1\u52d5', '\u5514\u90c1', '\u4e00\u8def\u90c1',
                 '\u591a\u8b1d\u6652', '\u63c0\u6652', '\u7747\u6652', '\u5571\u6652', '\u91cc\u5967'):
        if word in text:
            result = result.replace(converter.convert(word), word)
    # Preserve transliterated names and the correct interference/jamming term.
    # Blind repeated s2t conversion changes these already-Traditional spellings.
    for before, after in {
        '\u74e6\u723e\u57fa\u88cf': '\u74e6\u723e\u57fa\u91cc',
        '\u9054\u5e15\u88cf\u5967': '\u9054\u5e15\u91cc\u5967',
        '\u5e79\u64fe': '\u5e72\u64fe',
        '\u5168\u795e\u8cab\u6ce8\u4e8e': '\u5168\u795e\u8cab\u6ce8\u65bc',
    }.items():
        result = result.replace(before, after)
    return result
