namespace Audio {
    public readonly struct Note {
        public readonly float StartTime;
        public readonly float Frequency;
        public readonly float Volume;

        public Note(float startTime, float frequency, float volume) {
            this.StartTime = startTime;
            this.Frequency = frequency;
            this.Volume = volume;
        }
    }
}