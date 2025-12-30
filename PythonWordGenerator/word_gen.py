import csv
import nltk
import os  # <--- Added this library
from collections import defaultdict

def generate_balanced_word_list():
    # 1. Setup and Download
    print("Downloading dictionary data...")
    nltk.download('wordnet', quiet=True)
    from nltk.corpus import wordnet as wn

    TARGET_TOTAL = 5000
    ALPHABET = "abcdefghijklmnopqrstuvwxyz"
    TARGET_PER_LETTER = TARGET_TOTAL // 26 

    print(f"Targeting {TARGET_PER_LETTER} words per letter...")

    # 2. Collect words
    raw_data = defaultdict(dict)
    print("Scanning WordNet for 5-letter words...")
    
    for synset in wn.all_synsets():
        definition = synset.definition()
        for lemma in synset.lemmas():
            word = lemma.name().lower()
            if len(word) == 5 and word.isalpha() and word.isascii():
                start_char = word[0]
                if word not in raw_data[start_char]:
                    raw_data[start_char][word] = definition

    # 3. Balance the list
    final_list = []
    print("Balancing list...")
    
    for letter in ALPHABET:
        available_words = list(raw_data[letter].items())
        available_words.sort()
        
        if len(available_words) >= TARGET_PER_LETTER:
            selected = available_words[:TARGET_PER_LETTER]
        else:
            selected = available_words
            
        for word, definition in selected:
            final_list.append([word.title(), definition])

    # 4. Save to CSV (LOCKED TO SCRIPT LOCATION)
    # Get the directory where this python file is located
    script_dir = os.path.dirname(os.path.abspath(__file__))
    
    # Join that directory with the filename
    output_path = os.path.join(script_dir, 'balanced_5_letter_words.csv')
    
    print(f"Writing {len(final_list)} words to: {output_path}")
    
    with open(output_path, 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        writer.writerow(['Word', 'Description'])
        writer.writerows(final_list)

    print("Success!")

if __name__ == "__main__":
    generate_balanced_word_list()